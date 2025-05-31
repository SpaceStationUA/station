using Content.Shared._Pirate.Aiming.Events;
using Content.Shared.Hands;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Mobs.Components;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;
using Content.Shared.Popups;
using Content.Shared.Weapons.Ranged;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Timing;

namespace Content.Shared._Pirate.Aiming;

public sealed partial class SharedCanTakeAimSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedGunSystem _gun = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedInteractionSystem _interact = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CanTakeAimComponent, AfterInteractEvent>(OnWeaponTakeAim);
        SubscribeLocalEvent<CanTakeAimComponent, OnAimingTargetMoveEvent>(OnAimingTargetMove);
        SubscribeLocalEvent<CanTakeAimComponent, AmmoShotEvent>(OnAmmoShot);
        SubscribeLocalEvent<CanTakeAimComponent, DroppedEvent>(OnDropped);
    }
    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<CanTakeAimComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.User == null)
                continue;
            if (!comp.IsAiming)
                continue;
            foreach (var target in comp.AimingAt.ToArray())
            {
                if (!_interact.InRangeAndAccessible(comp.User.Value, target))
                {
                    if (!TryComp<OnSightComponent>(target, out var onSightComp))
                        continue;
                    RemoveComponentOnTarget(target, new Entity<CanTakeAimComponent>(uid, comp));
                    continue;
                }
            }
            UpdateIsAiming(comp);
        }
    }

    private void OnDropped(EntityUid uid, CanTakeAimComponent component, DroppedEvent args)
    {
        foreach (var target in component.AimingAt.ToArray())
        {
            RemoveComponentOnTarget(target, new Entity<CanTakeAimComponent>(uid, component));
        }
        UpdateIsAiming(component);
    }

    private void OnAmmoShot(EntityUid uid, CanTakeAimComponent comp, AmmoShotEvent args)
    {
        if (comp.User != null)
        {
            foreach (var entity in comp.AimingAt.ToArray())
            {
                RemoveComponentOnTarget(entity, new Entity<CanTakeAimComponent>(uid, comp));
            }
            comp.AimingAt.Clear();
            UpdateIsAiming(comp);
        }

    }

    private void OnAimingTargetMove(EntityUid uid, CanTakeAimComponent component, OnAimingTargetMoveEvent args)
    {
        if (!component.IsAiming)
            return;
        if (!component.AimingAt.Contains(args.Target))
            return;

        component.IsAiming = false;
        if (!TryComp<GunComponent>(uid, out var gunComp))
            return;
        var targetCoords = _transform.ToCoordinates(_transform.GetMapCoordinates(args.Target));
        var gunCoords = _transform.ToCoordinates(_transform.GetMapCoordinates(uid));
        ;
        // if (_gun.CanShoot(gunComp))
        // {
        EntityUid? ammo = null;
        if (TryComp<ChamberMagazineAmmoProviderComponent>(uid, out var chamberComp))
        {

            if (chamberComp.BoltClosed != null && chamberComp.BoltClosed.Value == false)
            {
                _popup.PopupClient(Loc.GetString("gun-chamber-bolt-ammo"), component.User, PopupType.Medium);
                return;
            }
            ammo = _gun.GetChamberEntity(uid);
        }
        if (TryComp<RevolverAmmoProviderComponent>(uid, out var revolverComp))
        {
            if (revolverComp.Chambers[revolverComp.CurrentIndex] == false)
            {
                _popup.PopupClient(Loc.GetString("gun-chamber-bolt-ammo"), component.User, PopupType.Medium);
                return;
            }
            var fromCoords = _transform.ToCoordinates(_transform.GetMapCoordinates(uid));
            var takeAmmoEvent = new TakeAmmoEvent(1, new List<(EntityUid? Entity, IShootable Shootable)>(), fromCoords, component.User);
            RaiseLocalEvent(uid, takeAmmoEvent);

            if (takeAmmoEvent.Ammo.Count > 0)
            {
                ammo = takeAmmoEvent.Ammo[0].Entity;
            }
        }
        if (TryComp<BallisticAmmoProviderComponent>(uid, out var ballisticComp))
        {

            // Use TakeAmmo to safely get ammo
            var fromCoords = _transform.ToCoordinates(_transform.GetMapCoordinates(uid));
            var takeAmmoEvent = new TakeAmmoEvent(1, new List<(EntityUid? Entity, IShootable Shootable)>(), fromCoords, component.User);
            RaiseLocalEvent(uid, takeAmmoEvent);
            if (takeAmmoEvent.Ammo.Count > 0)
            {
                ammo = takeAmmoEvent.Ammo[0].Entity;
            }
        }
        if (TryComp<MagazineAmmoProviderComponent>(uid, out var magazineComp))
        {
            var magEntity = _gun.GetMagazineEntity(uid);
            if (magEntity == null)
                return;
            var fromCoords = _transform.ToCoordinates(_transform.GetMapCoordinates(uid));
            var takeAmmoEvent = new TakeAmmoEvent(1, new List<(EntityUid? Entity, IShootable Shootable)>(), fromCoords, component.User);
            RaiseLocalEvent(uid, takeAmmoEvent);
            ammo = takeAmmoEvent.Ammo[0].Entity;
        }
        if (ammo == null)
            return;
        _gun.Shoot(uid, gunComp, ammo.Value, gunCoords, targetCoords, out _, component.User);
        if (component.User == null)
            return;
        if (ballisticComp != null)
        {
            // For ballistic guns, use the ActivateInWorldEvent to trigger cycling
            if (ballisticComp.AutoCycle)
            {
                var activateEvent = new ActivateInWorldEvent(component.User.Value, uid, true);
                RaiseLocalEvent(uid, activateEvent);
            }
        }
        else if (chamberComp != null)
        {
            if (chamberComp.AutoCycle)
            {
                var useEvent = new UseInHandEvent(component.User.Value);
                RaiseLocalEvent(uid, useEvent);
            }
        }
        // }
        // _gun.ShootProjectile(ammo.Value, direction, _physics.GetMapLinearVelocity(uid, physComp), uid);
    }
    public void OnWeaponTakeAim(EntityUid uid, CanTakeAimComponent component, ref AfterInteractEvent args)
    {
        component.User = args.User;
        if (args.Target == null)
            return;
        if (args.Target == args.User)
            return;
        if (!HasComp<MobMoverComponent>(args.Target))
            return;
        if (!args.CanReach)
        {
            _popup.PopupClient("Я не зможу попасти в ціль звідси...", args.User, PopupType.Medium);
            return;
        }
        if (!TryComp<MetaDataComponent>(args.User, out var userMetaComp) || !TryComp<MetaDataComponent>(args.Target, out var targetMetaComp))
            return;
        if (component.IsAiming)
        {
            if (HasComp<OnSightComponent>(args.Target))
            {
                RemoveComponentOnTarget(args.Target.Value, new Entity<CanTakeAimComponent>(uid, component));
                UpdateIsAiming(component);
            }
            return;
        }

        if (userMetaComp == null || targetMetaComp == null)
            return;
        if (TryComp<MobStateComponent>(args.Target, out var targetMobState) && targetMobState.CurrentState != Mobs.MobState.Alive)
            return;

        component.AimStartFrame = _timing.CurFrame;
        EnsureAimingAt(args.Target.Value, component);
        EnsureComponentOnTarget(args.Target.Value, uid, args.User);
        UpdateIsAiming(component);
        _popup.PopupPredicted($"{userMetaComp.EntityName} is aiming at {targetMetaComp.EntityName}!", args.Target.Value, args.Target.Value, PopupType.LargeCaution);

    }
    private void EnsureComponentOnTarget(EntityUid target, EntityUid uid, EntityUid userUid) // uid is uid of gun, not user!!!
    {
        EnsureComp<OnSightComponent>(target, out var onSigthComp);
        if (!onSigthComp.AimedAtWith.Contains(uid))
            onSigthComp.AimedAtWith.Add(uid);

        if (!onSigthComp.AimedAtBy.Contains(userUid))
            onSigthComp.AimedAtBy.Add(userUid);
    }
    private void RemoveComponentOnTarget(EntityUid target, Entity<CanTakeAimComponent> ent)
    {
        if (TryComp<MetaDataComponent>(ent.Comp.User, out var userMetaComp) && TryComp<MetaDataComponent>(target, out var targetMetaComp))
            if (userMetaComp != null && targetMetaComp != null)
                _popup.PopupPredicted($"{userMetaComp.EntityName} stopped aiming at {targetMetaComp.EntityName}.", target, target, PopupType.Large);
        if (ent.Comp.User == null)
            return;
        var ev = new OnAimerShootingEvent(ent.Owner, ent.Comp.User.Value);
        if (!TryComp<CanTakeAimComponent>(ev.Gun, out var canTakeAimComp))
            return;
        if (HasComp<OnSightComponent>(target))
        {
            RaiseLocalEvent(target, ev);
            canTakeAimComp.AimingAt.Remove(target);
            return;
        }
    }
    private void EnsureAimingAt(EntityUid target, CanTakeAimComponent component)
    {
        if (!component.AimingAt.Contains(target))
            component.AimingAt.Add(target);
    }
    private void UpdateIsAiming(CanTakeAimComponent comp)
    {
        if (comp.AimingAt.Count > 0)
            comp.IsAiming = true;
        else
            comp.IsAiming = false;
    }
}

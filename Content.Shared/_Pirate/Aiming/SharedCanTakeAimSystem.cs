using System.Linq;
using Content.Shared._Pirate.Actions.Events;
using Content.Shared.Alert;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Damage;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Movement.Components;
using Content.Shared.Popups;
using Content.Shared.Projectiles;
using Content.Shared.Weapons.Ranged;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Timing;

namespace Content.Shared._Pirate.Aiming;

public sealed partial class SharedCanTakeAimSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly EntityManager _entMan = default!;
    [Dependency] private readonly SharedGunSystem _gun = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CanTakeAimComponent, AfterInteractEvent>(OnWeaponTakeAim);
        SubscribeLocalEvent<CanTakeAimComponent, OnAimingTargetMoveEvent>(OnAimingTargetMove);
        SubscribeLocalEvent<CanTakeAimComponent, AmmoShotEvent>(OnAmmoShot);
    }
    private void OnAmmoShot(EntityUid uid, CanTakeAimComponent component, AmmoShotEvent args)
    {
        // TODO: Add damage multiplying
        // foreach (var entity in args.FiredProjectiles)
        // {
        // if (TryComp<ProjectileComponent>(entity, out var projectile))
        // {
        //     var deltaT = (_timing.CurFrame - component.AimStartFrame) * _timing.FramesPerSecondAvg;
        //     Logger.Debug($"Delta T: {deltaT}s");
        //     projectile.Damage *= component.MaxDamageMultiplier * (component.MaxAimTime / deltaT);

        //     Dirty(entity, projectile);
        // }
        // }
    }

    private void OnAimingTargetMove(EntityUid uid, CanTakeAimComponent component, OnAimingTargetMoveEvent args)
    {
        component.IsAiming = false;
        Dirty(uid, component);
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
            if (revolverComp.Chambers[revolverComp.CurrentIndex] != true)
            {
                _popup.PopupClient(Loc.GetString("gun-chamber-bolt-ammo"), component.User, PopupType.Medium);
                return;
            }
            var fromCoords = _transform.ToCoordinates(_transform.GetMapCoordinates(uid));
            var takeAmmoEvent = new TakeAmmoEvent(1, new List<(EntityUid? Entity, IShootable Shootable)>(), fromCoords, component.User);
            RaiseLocalEvent(uid, takeAmmoEvent);

            if (takeAmmoEvent.Ammo.Count > 0)
            {
                ammo = revolverComp.AmmoSlots[revolverComp.CurrentIndex];
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
                _gun.Shoot(uid, gunComp, takeAmmoEvent.Ammo[0].Entity!.Value, gunCoords, targetCoords, out _, component.User);
                return;
            }
            (ammo, _) = takeAmmoEvent.Ammo[0];
        }
        if (TryComp<MagazineAmmoProviderComponent>(uid, out var magazineComp))
        {
            var magEntity = _gun.GetMagazineEntity(uid);
            if (magEntity == null)
                return;
            var fromCoords = _transform.ToCoordinates(_transform.GetMapCoordinates(uid));
            var takeAmmoEvent = new TakeAmmoEvent(1, new List<(EntityUid? Entity, IShootable Shootable)>(), fromCoords, component.User);
            RaiseLocalEvent(uid, takeAmmoEvent);
            (ammo, _) = takeAmmoEvent.Ammo[0];
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
        else if (revolverComp != null)
        {
            // For revolvers, use the UseInHandEvent to trigger cycling
            var useEvent = new UseInHandEvent(component.User.Value);
            RaiseLocalEvent(uid, useEvent);
        }
        else if (chamberComp != null)
        {
            if (chamberComp.AutoCycle)
            {
                var useEvent = new UseInHandEvent(component.User.Value);
                RaiseLocalEvent(uid, useEvent);
            }
        }
        else
        {
            // For other gun types, try the CycleFire method which works for selective fire guns
            _gun.CycleFire(uid, gunComp, component.User);
        }
        // }
        // _gun.ShootProjectile(ammo.Value, direction, _physics.GetMapLinearVelocity(uid, physComp), uid);
    }
    public void OnWeaponTakeAim(EntityUid uid, CanTakeAimComponent component, ref AfterInteractEvent args)
    {
        if (args.Target == args.User)
            return;
        component.User = args.User;
        if (!args.CanReach)
        {
            _popup.PopupClient("Я не зможу попасти в ціль звідси...", args.User, PopupType.Medium);
            return;
        }
        if (!HasComp<MobMoverComponent>(args.Target))
            return;
        if (args.Target == null)
            return;
        if (!TryComp<MetaDataComponent>(args.User, out var userMetaComp) || !TryComp<MetaDataComponent>(args.Target, out var targetMetaComp))
            return;
        if (component.IsAiming)
        {
            if (HasComp<OnSightComponent>(args.Target))
            {
                RemComp<OnSightComponent>(args.Target.Value);
                _popup.PopupPredicted($"{userMetaComp.EntityName} stopped aiming at {targetMetaComp.EntityName}.", args.Target.Value, args.Target.Value, PopupType.Large);
                component.IsAiming = false;
            }
            return;
        }

        if (userMetaComp == null || targetMetaComp == null)
            return;
        if (TryComp<MobStateComponent>(args.Target, out var targetMobState) && targetMobState.CurrentState != Mobs.MobState.Alive)
            return;

        component.AimStartFrame = _timing.CurFrame;
        EnsureComponentOnTarget(args.Target.Value, uid);
        component.IsAiming = true;
        _popup.PopupPredicted($"{userMetaComp.EntityName} is aiming at {targetMetaComp.EntityName}!", args.Target.Value, args.Target.Value, PopupType.LargeCaution);

    }
    private void EnsureComponentOnTarget(EntityUid target, EntityUid uid) // uid is uid of gun, not user!!!
    {
        if (!HasComp<OnSightComponent>(target))
        {
            var onSigthComp = new OnSightComponent
            {
                AimedAtBy = new()
            };
            onSigthComp.AimedAtBy.Add(uid);
            AddComp(target, onSigthComp);
        }
        else
        {
            var onSigthComp = _entMan.GetComponent<OnSightComponent>(target);
            if (!onSigthComp.AimedAtBy.Contains(uid))
                onSigthComp.AimedAtBy.Add(uid);
        }
    }

}

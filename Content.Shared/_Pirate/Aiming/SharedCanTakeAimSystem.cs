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
        foreach (var entity in args.FiredProjectiles)
        {
            if (TryComp<ProjectileComponent>(entity, out var projectile))
            {
                var deltaT = (_timing.CurFrame - component.AimStartFrame) * _timing.FramesPerSecondAvg;
                Logger.Debug($"Delta T: {deltaT}s");
                projectile.Damage *= component.MaxDamageMultiplier * (component.MaxAimTime / deltaT);

                Dirty(entity, projectile);
            }
        }
    }

    private void OnAimingTargetMove(EntityUid uid, CanTakeAimComponent component, OnAimingTargetMoveEvent args)
    {
        Logger.Info("Target moved");
        component.IsAiming = false;
        Dirty(uid, component);
        if (!TryComp<GunComponent>(uid, out var gunComp))
            return;
        var targetCoords = _transform.ToCoordinates(_transform.GetMapCoordinates(args.Target));
        var gunCoords = _transform.ToCoordinates(_transform.GetMapCoordinates(uid));
        ;
        // if (_gun.CanShoot(gunComp))
        // {
        Logger.Info("Checking for Chamber..");
        if (TryComp<ChamberMagazineAmmoProviderComponent>(uid, out var chamberComp))
        {

            Logger.Info("Gun has chamber");
            if (chamberComp.BoltClosed != null && chamberComp.BoltClosed.Value == false)
            {
                Logger.Info("Gun is not bolted");
                _popup.PopupClient(Loc.GetString("gun-chamber-bolt-ammo"), component.User, PopupType.Medium);
                return;
            }
            var ammo = _gun.GetChamberEntity(uid);
            if (ammo == null)
                return;
            _gun.Shoot(uid, gunComp, ammo.Value, gunCoords, targetCoords, out _, component.User);
        }
        Logger.Info("Checking for Revolver..");
        if (TryComp<RevolverAmmoProviderComponent>(uid, out var revolverComp))
        {
            Logger.Info("Gun is revolver");
            Logger.Info($"CurrentIndex: {revolverComp.CurrentIndex}, state: {revolverComp.Chambers[revolverComp.CurrentIndex]}");
            if (revolverComp.Chambers[revolverComp.CurrentIndex] != true)
            {
                Logger.Info("Revolver is empty");
                _popup.PopupClient(Loc.GetString("gun-chamber-bolt-ammo"), component.User, PopupType.Medium);
                return;
            }
            var ammo = revolverComp.AmmoSlots[revolverComp.CurrentIndex];
            if (ammo == null)
                return;
            Logger.Info("Shooting revolver");
            _gun.Shoot(uid, gunComp, ammo.Value, gunCoords, targetCoords, out _, component.User);
        }
        if (component.User == null)
            return;
        if (TryComp<BallisticAmmoProviderComponent>(uid, out var ballisticComp))
        {
            // For ballistic guns, use the ActivateInWorldEvent to trigger cycling
            var activateEvent = new ActivateInWorldEvent(component.User.Value, uid, true);
            RaiseLocalEvent(uid, activateEvent);
        }
        else if (revolverComp != null)
        {
            // For revolvers, use the UseInHandEvent to trigger cycling
            var useEvent = new UseInHandEvent(component.User.Value);
            RaiseLocalEvent(uid, useEvent);
        }
        else if (chamberComp != null)
        {
            // For chamber-magazine guns, use the UseInHandEvent to trigger cycling
            var useEvent = new UseInHandEvent(component.User.Value);
            RaiseLocalEvent(uid, useEvent);
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
        if (component.IsAiming)
            return;
        if (!HasComp<MobMoverComponent>(args.Target))
            return;
        if (args.Target == null)
            return;
        if (!TryComp<MetaDataComponent>(args.User, out var userMetaComp) || !TryComp<MetaDataComponent>(args.Target, out var targetMetaComp))
            return;
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
        Log.Debug("Ensuring component on target...");
        if (!HasComp<OnSigthComponent>(target))
        {
            Logger.Debug("Target dont have OnSigthComponent. Adding it...");
            var onSigthComp = new OnSigthComponent
            {
                AimedAtBy = new()
            };
            onSigthComp.AimedAtBy.Add(uid);
            AddComp(target, onSigthComp);
        }
        else
        {
            Log.Debug("Target have OnSigthComponent. Adding user to it...");
            var onSigthComp = _entMan.GetComponent<OnSigthComponent>(target);
            if (!onSigthComp.AimedAtBy.Contains(uid))
                onSigthComp.AimedAtBy.Add(uid);
        }
    }

}

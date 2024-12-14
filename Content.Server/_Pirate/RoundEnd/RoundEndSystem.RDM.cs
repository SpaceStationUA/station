using Content.Server.Administration.Systems;
using Content.Server.GameTicking;
using Content.Server.Hands.Systems;
using Content.Server.Popups;
using Content.Server.Store.Systems;
using Content.Shared.Coordinates;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;


namespace Content.Server.DeltaV.RoundEnd;


public sealed class RDMRoundEnd : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly EntityManager _entityManager = default!;
    [Dependency] private readonly StoreSystem _storeSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly RejuvenateSystem _rejuvenate = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;


    //list of weapons possible to spawn
    HashSet<String> _weapons = new HashSet<String>
    {
        "WeaponRifleAk",
        "WeaponRifleLecter",
        "WeaponPistolViper",
        "WeaponPistolCobra",
        "WeaponPistolMk58",
        "WeaponPistolN1984",
        "WeaponRevolverDeckard",
        "WeaponRevolverInspector",
        "WeaponRevolverMateba",
        "WeaponRevolverPython",
        "WeaponRevolverPirate",
        "WeaponShotgunBulldog",
        "WeaponShotgunBulldogHoS",
        "WeaponShotgunDoubleBarreled",
        "WeaponShotgunEnforcer",
        "WeaponShotgunKammerer",
        "WeaponShotgunSawn",
        "WeaponShotgunImprovised",
        "WeaponSubMachineGunAtreides",
        "WeaponSubMachineGunC20r",
        "WeaponSubMachineGunC20rHoS",
        "WeaponSubMachineGunDrozd",
        "WeaponSubMachineGunVector",
        "WeaponSubMachineGunWt550",
        "WeaponSubMachineGunWt550HoS",
        "WeaponSniperMosin",
        "WeaponSniperHristov",
        "Musket",
        "WeaponPistolFlintlock",
        "WeaponEnergyGun",
        "WeaponEnergyGunMultiphase",
        "WeaponEnergyGunMini",
        "WeaponEnergyGunPistol",
        "WeaponGunLaserCarbineAutomatic",
        "WeaponEnergyGunAdvancedRevolver",
    };

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RoundEndTextAppendEvent>(OnRoundEnded);
    }

    private void OnRoundEnded(RoundEndTextAppendEvent ev)
    {
        var harmQuery = EntityQueryEnumerator<ActorComponent>();
        while (harmQuery.MoveNext(out var uid, out var _))
        {
            try
            {
                //is alive
                if (!EntityManager.TryGetComponent<MobStateComponent>(uid, out var mobState)
                    || mobState.CurrentState == MobState.Dead)
                {
                    continue;
                }

                //have 2 hands at least
                if (!EntityManager.TryGetComponent<HandsComponent>(uid, out var hands)
                    || hands.Count < 2)
                {
                    continue;
                }

                //heal them
                _rejuvenate.PerformRejuvenate(uid);

                //spawn weapons
                var weapon = _random.Pick(_weapons);
                var weaponItem = Spawn(weapon, uid.ToCoordinates());
                _hands.TryForcePickupAnyHand(uid, weaponItem);
            }
            catch (Exception e) { }
        }
    }
}

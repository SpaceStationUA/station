using Content.Server.GameTicking;
using Content.Shared.GameTicking;
using Robust.Server.GameObjects;
using Robust.Server.Maps;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Content.Server.Spawners.Components;
using Robust.Shared.Random;
using Robust.Server.Player;
using Content.Server.Chat.Systems;
using Content.Server.Station.Systems;
using Robust.Shared.Utility;
using Robust.Shared.Audio;
using System.Threading;
using Content.Server.Actions;
using Content.Server.Ghost.Roles.Components;
using Content.Server.RandomMetadata;
using Robust.Shared.Serialization.Manager;

namespace Content.Server._Pirate.SpecialForces;

public sealed class SpecialForcesSystem : EntitySystem
{
    // ReSharper disable once MemberCanBePrivate.Global
    [ViewVariables] public List<SpecialForcesHistory> CalledEvents { get; private set; } = new List<SpecialForcesHistory>();
    // ReSharper disable once MemberCanBePrivate.Global
    [ViewVariables] public TimeSpan LastUsedTime { get; private set; } = TimeSpan.Zero;

    private readonly TimeSpan _delayUsage = TimeSpan.FromMinutes(2);
    private readonly ReaderWriterLockSlim _callLock = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpecialForceComponent, MapInitEvent>(OnMapInit, after: new[] { typeof(RandomMetadataSystem) });
        SubscribeLocalEvent<RoundEndTextAppendEvent>(OnRoundEnd);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnCleanup);
        SubscribeLocalEvent<SpecialForceComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<SpecialForceComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnShutdown(EntityUid uid, SpecialForceComponent component, ComponentShutdown args)
    {
            _actions.RemoveAction(uid, component.BssKey);
    }

    private void OnStartup(EntityUid uid, SpecialForceComponent component, ComponentStartup args)
    {
        if (component.ActionBssActionName != null)
            _actions.AddAction(uid, ref component.BssKey, component.ActionBssActionName);
    }

    private void OnMapInit(EntityUid uid, SpecialForceComponent component, MapInitEvent args)
    {
        if (component.Components != null)
        {
            foreach (var entry in component.Components.Values)
            {
                var comp = (Component) _serialization.CreateCopy(entry.Component, notNullableOverride: true);
                comp.Owner = uid;
                EntityManager.AddComponent(uid, comp);
            }
        }
    }

    public TimeSpan DelayTime
    {
        get
        {
            var ct = _gameTicker.RoundDuration();
            var lastUsedTime = LastUsedTime + _delayUsage;
            return ct > lastUsedTime ? TimeSpan.Zero : lastUsedTime - ct;
        }
    }

    public bool CallOps(SpecialForcesType ev, string source = "")
    {
        _callLock.EnterWriteLock();
        try
        {
            if (_gameTicker.RunLevel != GameRunLevel.InRound)
            {
                return false;
            }

            var currentTime = _gameTicker.RoundDuration();

#if !DEBUG
            if (LastUsedTime + _delayUsage > currentTime)
            {
                return false;
            }
#endif

            LastUsedTime = currentTime;

            CalledEvents.Add(new SpecialForcesHistory { Event = ev, RoundTime = currentTime, WhoCalled = source });

            var shuttle = SpawnShuttle(ev);
            if (shuttle == null)
            {
                return false;
            }

            SpawnGhostRole(ev, shuttle.Value);

            PlaySound(ev);

            return true;
        }
        finally
        {
            _callLock.ExitWriteLock();
        }
    }

    private EntityUid SpawnEntity(string? protoName, EntityCoordinates coordinates)
    {
        if (protoName == null)
        {
            return EntityUid.Invalid;
        }

        var uid = EntityManager.SpawnEntity(protoName, coordinates);

        if (!TryComp<GhostRoleMobSpawnerComponent>(uid, out var mobSpawnerComponent) ||
            mobSpawnerComponent.Prototype == null ||
            !_prototypes.TryIndex<EntityPrototype>(mobSpawnerComponent.Prototype, out var spawnObj))
        {
            return uid;
        }

        if (spawnObj.TryGetComponent<SpecialForceComponent>(out var tplSpecForceComponent))
        {
            var comp = (Component) _serialization.CreateCopy(tplSpecForceComponent, notNullableOverride: true);
            comp.Owner = uid;
            EntityManager.AddComponent(uid, comp);
        }

        EnsureComp<SpecialForceComponent>(uid);
        if (spawnObj.TryGetComponent<GhostRoleComponent>(out var tplGhostRoleComponent))
        {
            var comp = (Component) _serialization.CreateCopy(tplGhostRoleComponent, notNullableOverride: true);
            comp.Owner = uid;
            EntityManager.AddComponent(uid, comp);
        }

        return uid;
    }

    private void SpawnGhostRole(SpecialForcesType ev, EntityUid shuttle)
    {
        var spawns = new List<EntityCoordinates>();

        foreach (var (_, meta, xform) in EntityManager
                     .EntityQuery<SpawnPointComponent, MetaDataComponent, TransformComponent>(true))
        {
            if (meta.EntityPrototype?.ID != SpawnMarker)
                continue;

            if (xform.ParentUid != shuttle)
                continue;

            spawns.Add(xform.Coordinates);
            break;
        }

        if (spawns.Count == 0)
        {
            spawns.Add(Transform(shuttle).Coordinates);
        }

        // TODO: Cvar
        var countExtra = _playerManager.PlayerCount switch
        {
            >= 60 => 7,
            >= 50 => 6,
            >= 40 => 5,
            >= 30 => 4,
            >= 20 => 3,
            >= 10 => 2,
            _ => 1
        };

        switch (ev)
        {
            case SpecialForcesType.ERT:
                SpawnEntity(ErtLeader, _random.Pick(spawns));
                // SpawnEntity(ErtEngineer, _random.Pick(spawns));

                while (countExtra > 0)
                {
                    if (countExtra-- > 0)
                    {
                        SpawnEntity(ErtSecurity, _random.Pick(spawns));
                    }

                    if (countExtra-- > 0)
                    {
                        SpawnEntity(ErtEngineer, _random.Pick(spawns));
                    }

                    if (countExtra-- > 0)
                    {
                        SpawnEntity(ErtMedical, _random.Pick(spawns));
                    }

                    if (countExtra-- > 0)
                    {
                        SpawnEntity(ErtJanitor, _random.Pick(spawns));
                    }
                }

                break;
            case SpecialForcesType.CBURN:
                // SpawnEntity(CburnLeader, _random.Pick(spawns));
                SpawnEntity(CburnFlamer, _random.Pick(spawns));
                while (countExtra > 0)
                {
                    if (countExtra-- > 0)
                    {
                        SpawnEntity(Cburn, _random.Pick(spawns));
                    }
                }

                break;
            case SpecialForcesType.DeathSquad:
                SpawnEntity(DeadsquadLeader, _random.Pick(spawns));
                while (countExtra > 0)
                {
                    if (countExtra-- > 0)
                    {
                        SpawnEntity(Deadsquad, _random.Pick(spawns));
                    }
                }

                break;
            default:
                return;
        }
    }

    private EntityUid? SpawnShuttle(SpecialForcesType ev)
    {
        var shuttleMap = _mapManager.CreateMap();
        var options = new MapLoadOptions()
        {
            LoadMap = true
        };

        if (!_map.TryLoad(shuttleMap,
                ev switch
                {
                    // todo: cvar
                    SpecialForcesType.ERT => EtrShuttlePath,
                    SpecialForcesType.CBURN => CburnShuttlePath,
                    SpecialForcesType.DeathSquad => DeadsquadShuttlePath,
                    _ => EtrShuttlePath
                },
                out var grids,
                options))
        {
            return null;
        }

        var mapGrid = grids.FirstOrNull();

        return mapGrid ?? null;
    }

    private void PlaySound(SpecialForcesType ev)
    {
        var stations = _stationSystem.GetStations();
        if (stations.Count == 0)
        {
            return;
        }

        switch (ev)
        {
            case SpecialForcesType.ERT:
                foreach (var station in stations)
                {
                    _chatSystem.DispatchStationAnnouncement(station,
                        Loc.GetString("spec-forces-system-ertcall-annonce"),
                        Loc.GetString("spec-forces-system-ertcall-title"),
                        false, _ertAnnounce
                    );
                }

                break;
            case SpecialForcesType.CBURN:
                foreach (var station in stations)
                {
                    _chatSystem.DispatchStationAnnouncement(station,
                        Loc.GetString("spec-forces-system-CBURN-annonce"),
                        Loc.GetString("spec-forces-system-CBURN-title"),
                        true
                    );
                }

                break;
            default:
                return;
        }
    }

    private void OnRoundEnd(RoundEndTextAppendEvent ev)
    {
        foreach (var calledEvent in CalledEvents)
        {
            ev.AddLine(Loc.GetString("spec-forces-system-" + calledEvent.Event,
                ("time", calledEvent.RoundTime.ToString(@"hh\:mm\:ss")), ("who", calledEvent.WhoCalled)));
        }
    }

    private void OnCleanup(RoundRestartCleanupEvent ev)
    {
        CalledEvents.Clear();
        LastUsedTime = TimeSpan.Zero;

        if (_callLock.IsWriteLockHeld)
        {
            _callLock.ExitWriteLock();
        }
    }

    [ValidatePrototypeId<EntityPrototype>] private const string SpawnMarker = "MarkerSpecialforce";
    private const string EtrShuttlePath = "Maps/Shuttles/dart.yml";
    [ValidatePrototypeId<EntityPrototype>] private const string ErtLeader = "RandomHumanoidSpawnerERTLeaderEVALecter";
    [ValidatePrototypeId<EntityPrototype>] private const string ErtSecurity = "RandomHumanoidSpawnerERTSecurityEVALecter";
    [ValidatePrototypeId<EntityPrototype>] private const string ErtEngineer = "RandomHumanoidSpawnerERTEngineerEVA";
    [ValidatePrototypeId<EntityPrototype>] private const string ErtJanitor = "RandomHumanoidSpawnerERTJanitorEVA";
    [ValidatePrototypeId<EntityPrototype>] private const string ErtMedical = "RandomHumanoidSpawnerERTMedicalEVA";

    private const string CburnShuttlePath = "Maps/Shuttles/dart.yml";
    [ValidatePrototypeId<EntityPrototype>] private const string CburnLeader = "RandomHumanoidSpawnerCBURNUnit";
    [ValidatePrototypeId<EntityPrototype>] private const string Cburn = "RandomHumanoidSpawnerCBURNUnit";
    [ValidatePrototypeId<EntityPrototype>] private const string CburnFlamer = "RandomHumanoidSpawnerCBURNFlamerUnit";

    private const string DeadsquadShuttlePath = "Maps/Shuttles/dart.yml";
    [ValidatePrototypeId<EntityPrototype>] private const string DeadsquadLeader = "RandomHumanoidSpawnerDeathSquad";
    [ValidatePrototypeId<EntityPrototype>] private const string Deadsquad = "RandomHumanoidSpawnerDeathSquad";

    private readonly SoundSpecifier _ertAnnounce = new SoundPathSpecifier("/Audio/Announcements/announce.ogg");

    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly MapLoaderSystem _map = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly ChatSystem _chatSystem = default!;
    [Dependency] private readonly StationSystem _stationSystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly ISerializationManager _serialization = default!;
    [Dependency] private readonly ActionsSystem _actions = default!;
}
using Content.Server.Magic;
// using Content.Server.Pulling;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.Cuffs.Components;
using Content.Shared.Damage.Systems;
// using Content.Shared.Pulling.Components;

using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Movement.Pulling;
using Content.Shared.Storage.Components;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Physics.Systems;
using Content.Shared.Magic.Events;
// using Content.Shared._Pirate.Mage.Events;
using Content.Shared._Pirate.Mage.Events;
using Content.Server._Pirate.Mage.EntitySystems;
using Content.Server._Pirate.Mage.Components;
using Content.Shared._Pirate.Mage.Components;

using System.Numerics;
using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Chat.Systems;
using Content.Server.Doors.Systems;
using Content.Server.Magic.Components;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.Actions;
using Content.Shared.Body.Components;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.DoAfter;
using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Content.Shared.Interaction.Events;
using Content.Shared.Magic;
using Content.Shared.Magic.Events;
using Content.Shared.Maps;
using Content.Shared.Physics;
using Content.Shared.Storage;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Random;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Spawners;

namespace Content.Server._Pirate.Mage.EntitySystems;

public sealed class MageProgectileSystem : EntitySystem
{
    [Dependency] private readonly MageManaSystem _mana = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly GunSystem _gunSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IEntityManager _entity = default!;
    [Dependency] private readonly StaminaSystem _stamina = default!;
    [Dependency] private readonly PullingSystem _pulling = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly MagicSystem _magic = default!;
    [Dependency] private readonly PhysicsSystem _physics = default!;
    [Dependency] private readonly SharedTransformSystem _transformSystem = default!;


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MageProjectileSpellEvent>(OnProjectileSpell);

    }

    private void OnProjectileSpell(MageProjectileSpellEvent ev)
    {
        if (!_entity.TryGetComponent<MageComponent>(ev.Performer, out var comp) || // Not a Mage
            _entity.TryGetComponent<HandcuffComponent>(ev.Performer, out var cuffs) || // handcuffed
            _entity.HasComponent<InsideEntityStorageComponent>(ev.Performer)) // Inside an entity storage
            return;

        if (ev.Handled)
            return;
        if (!_mana.TryUseAbility(ev.Performer, comp, ev.ManaCost))
            return;

        ev.Handled = true;
        _magic.Speak(ev);

        // Take power and deal stamina damage
        // _mana.TryAddPowerLevel(comp.Owner, -ev.ManaCost);

        var xform = Transform(ev.Performer);
        var userVelocity = _physics.GetMapLinearVelocity(ev.Performer);

        foreach (var pos in GetSpawnPositions(xform, ev.Pos))
        {
            // If applicable, this ensures the projectile is parented to grid on spawn, instead of the map.
            var mapPos = pos.ToMap(EntityManager);
            var spawnCoords = _mapManager.TryFindGridAt(mapPos, out var gridUid, out _)
                ? pos.WithEntityId(gridUid, EntityManager)
                : new(_mapManager.GetMapEntityId(mapPos.MapId), mapPos.Position);

            var ent = Spawn(ev.Prototype, spawnCoords);
            var direction = ev.Target.ToMapPos(EntityManager, _transformSystem) -
                            spawnCoords.ToMapPos(EntityManager, _transformSystem);
            _gunSystem.ShootProjectile(ent, direction, userVelocity, ev.Performer, ev.Performer);
        }
    }

    private List<EntityCoordinates> GetSpawnPositions(TransformComponent casterXform, MagicSpawnData data)
    {
        switch (data)
        {
            case TargetCasterPos:
                return new List<EntityCoordinates>(1) {casterXform.Coordinates};
            case TargetInFront:
            {
                // This is shit but you get the idea.
                var directionPos = casterXform.Coordinates.Offset(casterXform.LocalRotation.ToWorldVec().Normalized());

                if (!_mapManager.TryGetGrid(casterXform.GridUid, out var mapGrid))
                    return new List<EntityCoordinates>();

                if (!directionPos.TryGetTileRef(out var tileReference, EntityManager, _mapManager))
                    return new List<EntityCoordinates>();

                var tileIndex = tileReference.Value.GridIndices;
                var coords = mapGrid.GridTileToLocal(tileIndex);
                EntityCoordinates coordsPlus;
                EntityCoordinates coordsMinus;

                var dir = casterXform.LocalRotation.GetCardinalDir();
                switch (dir)
                {
                    case Direction.North:
                    case Direction.South:
                    {
                        coordsPlus = mapGrid.GridTileToLocal(tileIndex + (1, 0));
                        coordsMinus = mapGrid.GridTileToLocal(tileIndex + (-1, 0));
                        return new List<EntityCoordinates>(3)
                        {
                            coords,
                            coordsPlus,
                            coordsMinus,
                        };
                    }
                    case Direction.East:
                    case Direction.West:
                    {
                        coordsPlus = mapGrid.GridTileToLocal(tileIndex + (0, 1));
                        coordsMinus = mapGrid.GridTileToLocal(tileIndex + (0, -1));
                        return new List<EntityCoordinates>(3)
                        {
                            coords,
                            coordsPlus,
                            coordsMinus,
                        };
                    }
                }

                return new List<EntityCoordinates>();
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


}

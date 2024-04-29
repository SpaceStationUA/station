using Content.Server.Magic;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.Cuffs.Components;
using Content.Shared.Damage.Systems;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Movement.Pulling;
using Content.Shared.Storage.Components;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Physics.Systems;
using Content.Shared.Magic.Events;
using Content.Shared._Pirate.Mage.Events;
using Content.Server._Pirate.Mage.EntitySystems;
using Content.Server._Pirate.Mage.Components;
using Content.Shared._Pirate.Mage.Components;

namespace Content.Server._Pirate.Mage.EntitySystems;

public sealed class MageSwapSystem : EntitySystem
{
    [Dependency] private readonly MageManaSystem _mana = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IEntityManager _entity = default!;
    [Dependency] private readonly StaminaSystem _stamina = default!;
    [Dependency] private readonly PullingSystem _pulling = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly MagicSystem _magic = default!;
    [Dependency] private readonly SharedTransformSystem _transformSystem = default!;


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MageSwapSpellEvent>(OnSwapSpell);

    }

    /// <summary>
    /// Teleports the user to the clicked location
    /// </summary>
    /// <param name="args"></param>
    private void OnSwapSpell(MageSwapSpellEvent args)
    {
        if (!_entity.TryGetComponent<MageComponent>(args.Performer, out var comp) || // Not a Mage
            _entity.TryGetComponent<HandcuffComponent>(args.Performer, out var cuffs) || // handcuffed
            _entity.HasComponent<InsideEntityStorageComponent>(args.Performer)) // Inside an entity storage
            return;

        if (args.Handled)
            return;
        if (!_mana.TryUseAbility(args.Performer, comp, args.ManaCost))
            return;

        var transform = Transform(args.Performer);
        var coords = transform.Coordinates;

        var transformother = Transform(args.Target);
        var coordsother = transformother.Coordinates;

        _transformSystem.SetCoordinates(args.Performer, coordsother);
        _transformSystem.SetCoordinates(args.Target, coords);

        _magic.Speak(args);

        args.Handled = true;
    }

}

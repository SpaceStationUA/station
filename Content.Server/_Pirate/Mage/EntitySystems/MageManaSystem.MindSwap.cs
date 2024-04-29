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
using Content.Shared.StatusEffect;
using Content.Server.Mind;
using Robust.Shared.Map;

namespace Content.Server._Pirate.Mage.EntitySystems;

public sealed class MageMindSwapSystem : EntitySystem
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
    [Dependency] private readonly StatusEffectsSystem _statusEffects = default!;
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MageMindSwapSpellEvent>(OnMindswapSpell);

    }

    /// <summary>
    /// Teleports the user to the clicked location
    /// </summary>
    /// <param name="args"></param>
    private void OnMindswapSpell(MageMindSwapSpellEvent args)
    {
        if (!_entity.TryGetComponent<MageComponent>(args.Performer, out var comp) || // Not a Mage
            _entity.TryGetComponent<HandcuffComponent>(args.Performer, out var cuffs) || // handcuffed
            _entity.HasComponent<InsideEntityStorageComponent>(args.Performer)) // Inside an entity storage
            return;

        if (args.Handled)
            return;

        var transform = Transform(args.Performer);
        if (!_mapManager.TryFindGridAt(transform.MapPosition, out _, out var grid))
            return;

        if (!_mana.TryUseAbility(args.Performer, comp, args.ManaCost))
            return;

        _statusEffects.TryAddStatusEffect(args.Performer, "KnockedDown", TimeSpan.FromSeconds(5), true);
        _statusEffects.TryAddStatusEffect(args.Target, "KnockedDown", TimeSpan.FromSeconds(5), true);
        //Get the position of the player
        if (!_mind.TryGetMind(args.Performer, out var selfmindId, out var selfmind))
            return;

        _mind.ControlMob(args.Performer, args.Target);

        if (!_mind.TryGetMind(args.Target, out var targetmindId, out var targetmind))
            return;

       _mind.ControlMob(args.Target, args.Performer);
       Console.WriteLine("mindswap lawd");

        _magic.Speak(args);

        args.Handled = true;
    }

}

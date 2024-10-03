using Content.Server.Antag;
using Content.Server.GameTicking.Rules;
using Content.Server.GameTicking.Rules.Components;
using Content.Shared.Humanoid;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server._Pirate.MakeATraitor;

public sealed class MakeATraitorSystem : EntitySystem
{
    public enum TraitorType
    {
        Traitor = 0,
        Thief = 1,
        Revolutionary = 2
    }

    [ValidatePrototypeId<EntityPrototype>]
    private const string DefaultTraitorRule = "Traitor";

    [ValidatePrototypeId<EntityPrototype>]
    private const string DefaultRevsRule = "Revolutionary";

    [ValidatePrototypeId<EntityPrototype>]
    private const string DefaultThiefRule = "Thief";

    [Dependency] private readonly AntagSelectionSystem _antag = default!;

    public void MakeTraitor(TraitorType traitorType, EntityUid entity)
    {
        if (!TryComp<ActorComponent>(entity, out var actor))
            return;

        var player = actor.PlayerSession;

        switch (traitorType)
        {
            case TraitorType.Traitor:
                MakeTraitor(player);
                break;
            case TraitorType.Thief:
                MakeThief(player);
                break;
            case TraitorType.Revolutionary:
                MakeRevolutionary(player);
                break;
            default:
                return;
        }
    }

    private void MakeTraitor(ICommonSession? target)
    {
        _antag.ForceMakeAntag<TraitorRuleComponent>(target, DefaultTraitorRule);
    }

    private void MakeThief(ICommonSession? target)
    {
        _antag.ForceMakeAntag<ThiefRuleComponent>(target, DefaultThiefRule);
    }

    private void MakeRevolutionary(ICommonSession? target)
    {
        _antag.ForceMakeAntag<RevolutionaryRuleComponent>(target, DefaultRevsRule);

    }
}

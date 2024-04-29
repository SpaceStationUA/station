using Content.Server.GameTicking.Rules;
using Content.Shared.Humanoid;

namespace Content.Server._Pirate.MakeATraitor;

public sealed class MakeATraitorSystem : EntitySystem
{
    public enum TraitorType
    {
        Traitor = 0,
        Thief = 1,
        Revolutionary = 2
    }

    [Dependency] private readonly RevolutionaryRuleSystem _revolutionaryRule = default!;
    [Dependency] private readonly ThiefRuleSystem _thief = default!;
    [Dependency] private readonly TraitorRuleSystem _traitorRule = default!;

    public void MakeTraitor(TraitorType traitorType, EntityUid entity)
    {
        switch (traitorType)
        {
            case TraitorType.Traitor:
                MakeTraitor(entity);
                break;
            case TraitorType.Thief:
                MakeThief(entity);
                break;
            case TraitorType.Revolutionary:
                MakeRevolutionary(entity);
                break;
            default:
                return;
        }
    }

    private void MakeTraitor(EntityUid target)
    {
        var isHuman = EntityManager.HasComponent<HumanoidAppearanceComponent>(target);
        _traitorRule.MakeTraitorAdmin(target, isHuman, isHuman);
    }

    private void MakeThief(EntityUid target)
    {
        _thief.AdminMakeThief(target, true);
    }

    private void MakeRevolutionary(EntityUid target)
    {
        _revolutionaryRule.OnHeadRevAdmin(target);
    }
}

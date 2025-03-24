using Content.Server._DV.Terminator.Components;

namespace Content.Server._DV.Terminator.Systems;

/// <summary>
/// DeltaV - this is just used for paradox anomaly upstream doesnt use it anymore.
/// </summary>
public sealed class TerminatorSystem : EntitySystem
{
    public void SetTarget(Entity<TerminatorComponent?> ent, EntityUid mindId)
    {
        ent.Comp ??= EnsureComp<TerminatorComponent>(ent);
        ent.Comp.Target = mindId;
    }
}

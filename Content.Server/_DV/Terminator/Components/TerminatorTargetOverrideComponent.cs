using Content.Server._DV.Terminator.Systems;


namespace Content.Server._DV.Terminator.Components;

/// <summary>
/// Sets this objective's target to the exterminator's target override, if it has one.
/// If not it will be random.
/// </summary>
[RegisterComponent, Access(typeof(TerminatorTargetOverrideSystem))]
public sealed partial class TerminatorTargetOverrideComponent : Component
{
}

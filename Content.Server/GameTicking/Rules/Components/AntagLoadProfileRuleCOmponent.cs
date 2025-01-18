using Content.Shared.Humanoid.Prototypes;
using Robust.Shared.Prototypes;


namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// Makes this rules antags spawn a humanoid, either from the player's profile or a random one.
/// </summary>
[RegisterComponent]
public sealed partial class AntagLoadProfileRuleComponent : Component
{
    /// <summary>
    ///     Shitmed - Starlight Abductors: Species valid for the rule.
    /// </summary>
    [DataField]
    public ProtoId<SpeciesPrototype>? SpeciesHardOverride;
}

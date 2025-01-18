namespace Content.Server.Roles;

/// <summary>
///     Added to mind role entities to tag that they are an Abductor Victim.
/// </summary>
[RegisterComponent]
public sealed partial class AbductorVictimRoleComponent : Component
{
    public string Name => "AbductorVictimRole";
}

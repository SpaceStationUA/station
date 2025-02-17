using Content.Shared._White.Standing;
using Content.Shared.Standing;


namespace Content.Shared._White.Knockdown;

[RegisterComponent]
public sealed partial class KnockdownOnCollideComponent : Component
{
    [DataField]
    public DropHeldItemsBehavior Behavior = DropHeldItemsBehavior.NoDrop;
}

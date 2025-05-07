namespace Content.Server._Pirate.AttachedAccessories;

[RegisterComponent]
public sealed partial class CanHoldAccessoriesComponent : Component
{
    [DataField] public int MaxAccessories = 3;
    public List<IComponent?> OriginalComponents = new List<IComponent?>();
}

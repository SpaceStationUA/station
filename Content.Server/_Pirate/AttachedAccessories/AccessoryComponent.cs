namespace Content.Server._Pirate.AttachedAccessories;
[RegisterComponent]
/// <summary>
///   Allows to be inserted into entity with CanHoldAccessoriesComponent.
/// </summary>
public sealed partial class AccessoryComponent : Component
{
    [DataField] public string[]? PushComponents;
}

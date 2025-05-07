using Content.Shared.Tag;
using Robust.Shared.Prototypes;

namespace Content.Server._Pirate.AttachedAccessories;

[RegisterComponent]
/// <summary>
///     Allows entity to hold accessories.
///     <seealso cref="AccessoryComponent"/>
/// </summary>
public sealed partial class CanHoldAccessoriesComponent : Component
{
    [DataField] public int MaxAccessories = 3;
    /// <summary>
    ///     Accessory must have at least one of these tags to be inserted.
    /// </summary>
    [DataField] public List<ProtoId<TagPrototype>>? WhiteListTags = new();
    [DataField] public List<ProtoId<TagPrototype>>? BlackListTags = new();

    public List<IComponent?> OriginalComponents = new();
}

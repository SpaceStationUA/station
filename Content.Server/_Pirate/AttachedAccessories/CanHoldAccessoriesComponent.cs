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
    ///     If null, no accessories can be inserted.
    /// </summary>
    [DataField] public List<ProtoId<TagPrototype>>? WhiteListTags = new();
    public List<IComponent?> OriginalComponents = new();
}

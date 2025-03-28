using Content.Shared.Emag.Systems;
using Content.Shared.Tag;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization;
using Content.Shared.Whitelist; // Shitmed - Starlight Abductors

namespace Content.Shared.Emag.Components;

[Access(typeof(EmagSystem))]
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class EmagComponent : Component
{
    /// <summary>
    /// The tag that marks an entity as immune to emags
    /// </summary>
    [DataField("emagImmuneTag", customTypeSerializer: typeof(PrototypeIdSerializer<TagPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public string EmagImmuneTag = "EmagImmune";

    /// <summary>
    ///     Shitmed - Starlight Abductors: Entities that this EMAG works on.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityWhitelist? ValidTargets;
    
    // DeltaV - Add a whitelist/blacklist to the Emag
    /// <summary>
    /// Whitelist that entities must be on to work.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Blacklist that entities must be off to work.
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;
    // End of DeltaV code
}

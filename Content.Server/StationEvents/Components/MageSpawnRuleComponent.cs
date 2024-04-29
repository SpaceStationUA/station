using Content.Server.StationEvents.Events;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(MageSpawnRule))]
public sealed partial class MageSpawnRuleComponent : Component
{
    [DataField("mageShuttlePath")]
    public string MageShuttlePath = "Maps/Shuttles/wizard.yml";

    // [DataField("gameRuleProto", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    // public string GameRuleProto = "Nukeops";

    [DataField("additionalRule")]
    public EntityUid? AdditionalRule;
}

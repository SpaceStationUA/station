using Robust.Shared.GameStates;
using Robust.Shared.Serialization; //PIRATE
using Content.Shared.Actions; //PIRATE
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype; //PIRATE

using Robust.Shared.Prototypes;

namespace Content.Shared.Emoting;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class EmotingComponent : Component
{
    [DataField, AutoNetworkedField]
    [Access(typeof(EmoteSystem), Friend = AccessPermissions.ReadWrite, Other = AccessPermissions.Read)]
    public bool Enabled = true;
    
    /// <summary>
    /// PIRATE Open emotes action id
    /// </summary>
    [DataField("openEmotesAction", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string OpenEmotesAction = "OpenEmotes";

    /// <summary>
    /// PIRATE Used for open emote menu action button
    /// </summary>
    // [DataField("action")]
    // public string? Action = ;

    [DataField("openEmotesActionEntity")] public EntityUid? ActionEntity;
}

//PIRATE START
[Serializable, NetSerializable]
public sealed partial class RequestEmoteMenuEvent : EntityEventArgs
{
    public readonly List<string> Prototypes = new();
    public NetEntity Target { get; }

    public RequestEmoteMenuEvent(NetEntity target)
    {
        Target = target;
    }
}

[Serializable, NetSerializable]
public sealed partial class SelectEmoteEvent : EntityEventArgs
{
    public string PrototypeId { get; }
    public NetEntity Target { get; }

    public SelectEmoteEvent(NetEntity target, string prototypeId)
    {
        Target = target;
        PrototypeId = prototypeId;
    }
}

public sealed partial class OpenEmotesActionEvent : InstantActionEvent
{
}
//PIRATE END
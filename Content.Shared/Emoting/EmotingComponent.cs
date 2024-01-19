using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Content.Shared.Actions;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
// using Content.Shared.Actions;
using Robust.Shared.Prototypes;

namespace Content.Shared.Emoting;

// [Serializable, NetSerializable]
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class EmotingComponent : Component
{
    [DataField, AutoNetworkedField]
    [Access(typeof(EmoteSystem), Friend = AccessPermissions.ReadWrite, Other = AccessPermissions.Read)]
    public bool Enabled = true;

    /// <summary>
    /// Open emotes action id
    /// </summary>
    [DataField("openEmotesAction", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string OpenEmotesAction = "OpenEmotes";

    /// <summary>
    /// Used for open emote menu action button
    /// </summary>
    // [DataField("action")]
    // public string? Action = ;

    [DataField("openEmotesActionEntity")] public EntityUid? ActionEntity;


}

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
using Robust.Shared.Serialization;

namespace Content.Shared.SimpleStation14.Species.Shadowkin.Events;

/// <summary>
///     Raised over network to notify the client that they're going in/out of The Dark.
/// </summary>
[Serializable, NetSerializable]
public sealed class ShadowkinDarkSwappedEvent : EntityEventArgs
{
    public NetEntity? Performer { get; set; }
    public bool DarkSwapped { get; }

    public ShadowkinDarkSwappedEvent(NetEntity performer, bool darkSwapped)
    {
        Performer = performer;
        DarkSwapped = darkSwapped;
    }
}

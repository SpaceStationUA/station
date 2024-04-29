namespace Content.Server.GameTicking.Events;

public sealed class RoundEndedEvent : EntityEventArgs
{
    public int RoundId { get; }
    public TimeSpan RoundDuration { get; }

    public RoundEndedEvent(int roundId, TimeSpan roundDuration)
    {
        RoundId = roundId;
        RoundDuration = roundDuration;
    }
}

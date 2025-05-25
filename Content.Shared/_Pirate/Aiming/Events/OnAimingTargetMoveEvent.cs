namespace Content.Shared._Pirate.Aiming.Events;

/// <summary>
/// Raised when an entity that is being aimed at moves.
/// </summary>
public sealed class OnAimingTargetMoveEvent : EntityEventArgs
{
    public readonly EntityUid Target;

    public OnAimingTargetMoveEvent(EntityUid target)
    {
        Target = target;
    }

}

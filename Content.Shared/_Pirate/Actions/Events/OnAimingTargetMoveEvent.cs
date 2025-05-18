using Robust.Shared.GameObjects;

namespace Content.Shared._Pirate.Actions.Events;

// This is a local event, not a networked event
public sealed class OnAimingTargetMoveEvent : EntityEventArgs
{
    public readonly EntityUid Target;

    public OnAimingTargetMoveEvent(EntityUid target)
    {
        Target = target;
    }
}

namespace Content.Shared._Pirate.Aiming;

/// <summary>
/// Component that is added to an entity when it is aimed at by another entity.
/// It is used to add alert and process OnAimingTargetMoveEvent
/// </summary>
[RegisterComponent]
public sealed partial class OnSightComponent : Component
{
    /// <summary>
    /// List of guns uids that are aiming at this entity.
    /// </summary>
    public List<EntityUid> AimedAtWith = new();
    /// <summary>
    /// List of actual players that are aiming at this entity.
    /// </summary>
    public List<EntityUid> AimedAtBy = new();

}

namespace Content.Shared._Pirate.Aiming;

/// <summary>
/// Component that is added to an entity when it is aimed at by another entity.
/// It is used to add alert and process OnAimingTargetMoveEvent
/// </summary>
[RegisterComponent]
public sealed partial class OnSigthComponent : Component
{
    /// <summary>
    /// List of entities that are aiming at this entity.
    /// </summary>
    [DataField] public List<EntityUid> AimedAtBy = new();
}

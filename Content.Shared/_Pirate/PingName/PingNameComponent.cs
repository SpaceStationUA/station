using Robust.Shared.GameStates;

namespace Content.Shared._Pirate.PingName;

/// <summary>
/// Component that enables name highlighting for a player.
/// When a player has this component, their name parts will be highlighted in chat messages.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PingNameComponent : Component
{
    /// <summary>
    /// The player's name parts (first name, last name, etc.) that should be highlighted.
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<string> NameParts = new();

    /// <summary>
    /// The root forms of the name parts used for matching.
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<string> NameRoots = new();

    /// <summary>
    /// Whether name highlighting is enabled for this player.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Enabled = true;

    public override bool SessionSpecific => true;
}

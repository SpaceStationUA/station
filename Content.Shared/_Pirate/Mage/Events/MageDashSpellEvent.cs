using Content.Shared.Actions;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Content.Shared.Magic;

namespace Content.Shared._Pirate.Mage.Events;

public sealed partial class MageDashSpellEvent : WorldTargetActionEvent, ISpeakSpell
{
    [DataField("sound")]
    public SoundSpecifier Sound = new SoundPathSpecifier("/Audio/_Park/Effects/Shadowkin/Powers/teleport.ogg");

    [DataField("speech")]
    public string? Speech { get; set;}

    [DataField("volume")]
    public float Volume = 5f;

    /// <summary>
    /// How much mana should be drained.
    /// </summary>
    [DataField("manaCost")]
    public float ManaCost = 20f;
}

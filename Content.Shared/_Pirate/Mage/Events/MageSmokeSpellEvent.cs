using Content.Shared.Actions;
using Robust.Shared.Audio;
using Content.Shared.Magic;

namespace Content.Shared._Pirate.Mage.Events;

public sealed partial class MageSmokeSpellEvent : InstantActionEvent, ISpeakSpell
{

    [DataField("sound")]
    public SoundSpecifier Sound = new SoundPathSpecifier("/Audio/Effects/smoke.ogg");

    [DataField("volume")]
    public float Volume = 5f;

    [DataField("speech")]
    public string? Speech { get; private set; }

    /// <summary>
    /// How much mana should be drained.
    /// </summary>
    [DataField("manaCost")]
    public float ManaCost = 20f;

}

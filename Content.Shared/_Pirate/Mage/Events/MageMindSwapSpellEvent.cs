using Content.Shared.Actions;
using Content.Shared.Magic;

namespace Content.Shared._Pirate.Mage.Events;

public sealed partial class MageMindSwapSpellEvent : EntityTargetActionEvent, ISpeakSpell
{
    [DataField]
    public float stunTime = 5f;

    [DataField("speech")]
    public string? Speech { get; private set; }

    /// <summary>
    /// How much mana should be drained.
    /// </summary>
    [DataField("manaCost")]
    public float ManaCost = 20f;

}

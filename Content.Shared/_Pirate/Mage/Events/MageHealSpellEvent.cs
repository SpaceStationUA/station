using Content.Shared.Actions;
using Content.Shared.Damage;
using Content.Shared.Magic;

namespace Content.Shared._Pirate.Mage.Events;

public sealed partial class MageHealSpellEvent : EntityTargetActionEvent, ISpeakSpell
{

    [DataField("speech")]
    public string? Speech { get; private set; }

    /// <summary>
    /// How much mana should be drained.
    /// </summary>
    [DataField("manaCost")]
    public float ManaCost = 20f;
}

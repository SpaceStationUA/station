﻿using Content.Shared.Actions;
using Content.Shared.Chat;
using Robust.Shared.Prototypes;

namespace Content.Shared.Magic.Events;

public sealed partial class ProjectileSpellEvent : EntityWorldTargetActionEvent, ISpeakSpell // Goob edit
{
    /// <summary>
    /// What entity should be spawned.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Prototype;

    [DataField]
    public string? Speech { get; private set; }

    public InGameICChatType ChatType { get; } = InGameICChatType.Speak;
}

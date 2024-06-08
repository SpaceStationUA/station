using Content.Server.GameTicking;
using Content.Shared.GameTicking;
using Robust.Server.GameObjects;
using Robust.Server.Maps;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Content.Server.Spawners.Components;
using Robust.Shared.Random;
using Robust.Server.Player;
using Content.Server.Chat.Systems;
using Content.Server.Station.Systems;
using Robust.Shared.Utility;
using Robust.Shared.Audio;
using System.Threading;
using Content.Server.Actions;
using Content.Server.Chat.Managers;
using Content.Server.Ghost.Roles.Components;
using Content.Server.RandomMetadata;
using Robust.Shared.Serialization.Manager;

namespace Content.Server._Pirate.SandboxRules;

public sealed class SandboxRulesSystem : EntitySystem
{
    [Dependency] private readonly IChatManager _chatManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerBeforeSpawnEvent>(OnPlayerSpawningEvent);
    }

    private void OnPlayerSpawningEvent(PlayerBeforeSpawnEvent ev)
    {
        _chatManager.DispatchServerMessage(ev.Player, Loc.GetString("game-ticker-player-join-game-message-sandbox"));
    }
}

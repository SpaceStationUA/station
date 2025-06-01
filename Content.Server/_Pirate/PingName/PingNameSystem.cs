using Content.Server.Mind;
using Content.Shared._Pirate.PingName;
using Content.Shared.GameTicking;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Players;
using Robust.Server.Player;
using Robust.Shared.Log;
using Robust.Shared.Player;

namespace Content.Server._Pirate.PingName;

public sealed class PingNameSystem : SharedPingNameSystem
{
    [Dependency] private readonly MindSystem _mindSystem = default!;

    private ISawmill _sawmill = default!;

    public override void Initialize()
    {
        base.Initialize();
        _sawmill = Logger.GetSawmill("pirate.ping_name");

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawnComplete);
        SubscribeLocalEvent<MindAddedMessage>(OnMindAdded);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
    }

    /// <summary>
    /// When a player spawns, set up their ping name component.
    /// </summary>
    private void OnPlayerSpawnComplete(PlayerSpawnCompleteEvent ev)
    {
        _sawmill.Debug($"Player spawn complete: {ev.Player.Name} -> {ev.Mob}");
        SetupPingNameForPlayer(ev.Player, ev.Mob);
    }

    /// <summary>
    /// When a mind is added to an entity, set up ping name component.
    /// </summary>
    private void OnMindAdded(MindAddedMessage ev)
    {
        if (_mindSystem.TryGetSession(ev.Mind, out var session))
        {
            SetupPingNameForPlayer(session, ev.Mind);
        }
    }

    /// <summary>
    /// Clean up ping name components on round restart.
    /// </summary>
    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        var query = EntityQueryEnumerator<PingNameComponent>();
        while (query.MoveNext(out var uid, out _))
        {
            RemComp<PingNameComponent>(uid);
        }
    }

    /// <summary>
    /// Sets up the ping name component for a player.
    /// </summary>
    private void SetupPingNameForPlayer(ICommonSession session, EntityUid mindEntity)
    {
        _sawmill.Debug($"Setting up ping name for player: {session.Name} (UserId: {session.UserId}) -> Mind: {mindEntity}");

        if (!_mindSystem.TryGetMind(session.UserId, out var mindId) || mindId.Value.Owner != mindEntity)
        {
            _sawmill.Debug($"Failed to get mind or mind owner mismatch. HasMind: {_mindSystem.TryGetMind(session.UserId, out _)}, MindOwner: {mindId?.Owner}");
            return;
        }

        // Add the component to the mind entity
        var comp = EnsureComp<PingNameComponent>(mindEntity);
        _sawmill.Debug($"Added PingNameComponent to mind entity: {mindEntity}");

        // Get the player's character name
        if (session.AttachedEntity is { Valid: true } playerEntity &&
            TryComp<MetaDataComponent>(playerEntity, out var meta))
        {
            _sawmill.Debug($"Found player entity: {playerEntity} with name: '{meta.EntityName}'");
            UpdateNameParts(mindEntity, meta.EntityName, comp);
        }
        else
        {
            _sawmill.Debug($"No valid player entity or metadata. AttachedEntity: {session.AttachedEntity}");
        }
    }

    /// <summary>
    /// Updates the ping name component when a player's name changes.
    /// </summary>
    public void UpdatePlayerName(ICommonSession session, string newName)
    {
        if (!_mindSystem.TryGetMind(session.UserId, out var mindId))
            return;

        if (TryComp<PingNameComponent>(mindId.Value.Owner, out var comp))
        {
            UpdateNameParts(mindId.Value.Owner, newName, comp);
        }
    }

    /// <summary>
    /// Enables or disables name highlighting for a player.
    /// </summary>
    public void SetPingNameEnabled(ICommonSession session, bool enabled)
    {
        if (!_mindSystem.TryGetMind(session.UserId, out var mindId))
            return;

        var comp = EnsureComp<PingNameComponent>(mindId.Value.Owner);
        comp.Enabled = enabled;
        Dirty(mindId.Value.Owner, comp);
    }

    /// <summary>
    /// Gets whether name highlighting is enabled for a player.
    /// </summary>
    public bool IsPingNameEnabled(ICommonSession session)
    {
        if (!_mindSystem.TryGetMind(session.UserId, out var mindId))
            return false;

        return TryComp<PingNameComponent>(mindId.Value.Owner, out var comp) && comp.Enabled;
    }
}

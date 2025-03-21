using Content.Server._Pirate.ForcePacify;
using Content.Server.GameTicking;
using Content.Server.Players.PlayTimeTracking;
using Content.Shared.CombatMode.Pacification;
using Content.Shared.GameTicking;
using Robust.Server.Player;

namespace Content.Server._Pirate.PacifiedNewbies;

public sealed class PacifiedNewbiesSystem : EntitySystem
{
    [Dependency] private readonly PlayTimeTrackingManager _playTimeTracking = default!;
    private static readonly PiratePacifyManager _piratePacifyManager = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawningEvent);
        SubscribeLocalEvent<GhostRoleTakenPirate>(OnPlayerGhostRoleTaken);
    }

    private void OnPlayerGhostRoleTaken(GhostRoleTakenPirate ev)
    {
        var overall = _playTimeTracking.GetOverallPlaytime(ev.Player);
        if (overall.TotalHours <= 1 || _piratePacifyManager.IsPacified(ev.Player.Name))
        {
            EnsureComp<PacifiedComponent>(ev.Mob);
        }
    }

    private void OnPlayerSpawningEvent(PlayerSpawnCompleteEvent ev)
    {
        var overall = _playTimeTracking.GetOverallPlaytime(ev.Player);
        if (overall.TotalHours <= 1 || _piratePacifyManager.IsPacified(ev.Player.Name))
        {
            EnsureComp<PacifiedComponent>(ev.Mob);
        }
    }
}

using Robust.Shared.Player;

namespace Content.Server._Pirate.PacifiedNewbies;

public record struct GhostRoleTakenPirate(ICommonSession Player, EntityUid Mob)
{
    public ICommonSession Player { get; } = Player;
    public EntityUid Mob { get; } = Mob;
}

using System.Linq;
using Content.Server.Standing;
using Content.Server.Stunnable.Components;
using Content.Shared.StatusEffect;
using Content.Shared.Stunnable.Events;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Server.Stunnable.Systems;

public sealed class KnockdownOnHitSystem : EntitySystem
{
    [Dependency] private readonly StunSystem _stun = default!;
    [Dependency] private readonly LayingDownSystem _laying = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<KnockdownOnHitComponent, MeleeHitEvent>(OnMeleeHit);
    }

    private void OnMeleeHit(Entity<KnockdownOnHitComponent> entity, ref MeleeHitEvent args)
    {
        if (args.Direction.HasValue || !args.IsHit || !args.HitEntities.Any() || entity.Comp.Duration <= TimeSpan.Zero)
            return;

        var ev = new KnockdownOnHitAttemptEvent();
        RaiseLocalEvent(entity, ref ev);
        if (ev.Cancelled)
            return;

        foreach (var target in args.HitEntities)
        {
            if (entity.Comp.Duration <= TimeSpan.Zero) // Goobstation
            {
                _laying.TryLieDown(target, null, null);
                continue;
            }

            if (!TryComp(target, out StatusEffectsComponent? statusEffects))
                continue;

            _stun.TryKnockdown(target,
                entity.Comp.Duration,
                entity.Comp.RefreshDuration,
                entity.Comp.DropHeldItemsBehavior,
                statusEffects);
        }
    }
}

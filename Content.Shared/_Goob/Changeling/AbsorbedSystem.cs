using Content.Shared.Examine;
using Content.Shared.Mobs;


namespace Content.Shared._Goob.Changeling;

public sealed partial class AbsorbedSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<_Goob.Changeling.AbsorbedComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<_Goob.Changeling.AbsorbedComponent, MobStateChangedEvent>(OnMobStateChange);
    }

    private void OnExamine(Entity<_Goob.Changeling.AbsorbedComponent> ent, ref ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("changeling-absorb-onexamine"));
    }

    private void OnMobStateChange(Entity<_Goob.Changeling.AbsorbedComponent> ent, ref MobStateChangedEvent args)
    {
        // in case one somehow manages to dehusk someone
        if (args.NewMobState != MobState.Dead)
            RemComp<_Goob.Changeling.AbsorbedComponent>(ent);
    }
}

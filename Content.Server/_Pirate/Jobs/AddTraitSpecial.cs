using Content.Server.Traits;
using Content.Shared.Roles;
using Content.Shared.Traits;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server._Pirate.Jobs
{
    [UsedImplicitly]
    public sealed partial class AddTraitSpecial : JobSpecial
    {
        [DataField("traits")]
        [AlwaysPushInheritance]
        public HashSet<String> Traits { get; private set; } = new();

        public override void AfterEquip(EntityUid mob)
        {
            var entMan = IoCManager.Resolve<IEntityManager>();
            var traitSystem = entMan.System<TraitSystem>();
            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();

            foreach (var trait in Traits)
            {
                if (!prototypeManager.TryIndex<TraitPrototype>(trait, out var _traitPrototype))
                {
                    Logger.Warning($"Trait '{trait}' does not exist in the prototype manager.");
                    continue;
                }

                traitSystem.AddTrait(mob, _traitPrototype);
            }
        }
    }
}

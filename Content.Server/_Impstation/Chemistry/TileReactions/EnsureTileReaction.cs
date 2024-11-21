using Content.Server.Fluids.EntitySystems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Content.Shared.Tag;
using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server.Chemistry.TileReactions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class EnsureTileReaction : ITileReaction
    {
        [DataField, ViewVariables]
        public ComponentRegistry Components = new();

        [DataField, ViewVariables]
        public HashSet<ProtoId<TagPrototype>> Tags = new();

        [DataField, ViewVariables]
        public bool Override = false;

        public FixedPoint2 TileReact(TileRef tile,
            ReagentPrototype reagent,
            FixedPoint2 reactVolume)
        {
            var entMan = IoCManager.Resolve<IEntityManager>();

            if (reactVolume < 5)
                return FixedPoint2.Zero;

            if (entMan.EntitySysManager.GetEntitySystem<PuddleSystem>()
                .TrySpillAt(tile, new Solution(reagent.ID, reactVolume), out var puddleUid, false, false))
            {
                entMan.AddComponents(puddleUid, Components, Override);
                // entMan.EntitySysManager.GetEntitySystem<TagSystem>().AddTags(puddleUid, Tags);

                return reactVolume;
            }

            return FixedPoint2.Zero;
        }
    }
}

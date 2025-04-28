using Content.Server._Pirate.ReactionChamber.Components;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Shared._Pirate.ReactionChamber;
using Content.Shared._Pirate.ReactionChamber.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Mobs;
using MathNet.Numerics;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server._Pirate.ReactionChamber.EntitySystems;
public sealed partial class ReactionChamberSystem : EntitySystem
{
    [Dependency] protected readonly IPrototypeManager PrototypeManager = default!;
    [Dependency] UserInterfaceSystem _userInterfaceSystem = default!;
    [Dependency] ItemSlotsSystem _itemSlotsSystem = default!;
    [Dependency]
    SharedSolutionContainerSystem _solutionContainerSystem = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<ReactionChamberComponent, EntInsertedIntoContainerMessage>(OnItemInsert);
        SubscribeLocalEvent<ReactionChamberComponent, ReactionChamberActiveChangeMessage>(onActiveChangeMessage);
        SubscribeLocalEvent<ReactionChamberComponent, ReactionChamberTempChangeMessage>(onTempChangeMessage);
    }
    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        bool isAllTempRight = false;
        var query = EntityQueryEnumerator<ReactionChamberComponent>();
        while (query.MoveNext(out var uid, out ReactionChamberComponent? comp) && comp.Active)
        {
            var beaker = _itemSlotsSystem.GetItemOrNull(uid, "beakerSlot");
            if (beaker is null)
                return;
            if (!TryComp<SolutionContainerManagerComponent>(beaker, out var container))
                continue;
            if (!isAllTempRight)
                foreach (var (_, soln) in _solutionContainerSystem.EnumerateSolutions(container.Owner)) // add temp to all solutions
                {
                    var (_, solnComp) = soln;
                    double deltaJ; // delta Juoles
                    float C = solnComp.Solution.GetHeatCapacity(PrototypeManager); //pytoma teploemnist
                    float solnTemp = solnComp.Solution.GetThermalEnergy(PrototypeManager) / C;
                    float deltaT = comp.Temp - solnTemp;
                    if (solnTemp != comp.Temp)
                    {
                        deltaJ = deltaT * C * frameTime * comp.BaseMultiplyer;
                        Log.Info($"{deltaJ} = {deltaT} * {C} * {frameTime} * {comp.BaseMultiplyer}");
                        _solutionContainerSystem.AddThermalEnergy(soln, (float) deltaJ);
                        isAllTempRight = false;
                    }
                    else
                    {
                        isAllTempRight = true;
                        return;
                    }
                }

            // comp.Active = false;
        }
    }
    void OnItemInsert<T>(Entity<ReactionChamberComponent> ent, ref T ev)
    {

        var beaker = _itemSlotsSystem.GetItemOrNull(ent, "beakerSlot");
        if (beaker is null)
            return;
        // ent.Comp.Active = true;
    }
    private void onActiveChangeMessage(Entity<ReactionChamberComponent> ent, ref ReactionChamberActiveChangeMessage args)
    {
        ent.Comp.Active = args.Active;
    }
    private void onTempChangeMessage(Entity<ReactionChamberComponent> ent, ref ReactionChamberTempChangeMessage args)
    {
        Log.Info($"Recived temp: {args.Temp}");
        ent.Comp.Temp = args.Temp;
    }
}

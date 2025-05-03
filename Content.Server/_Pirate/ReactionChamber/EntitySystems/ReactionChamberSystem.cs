using Content.Server._Pirate.ReactionChamber.Components;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Shared._Pirate.ReactionChamber;
using Content.Shared._Pirate.ReactionChamber.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.Temperature;
using MathNet.Numerics;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server._Pirate.ReactionChamber.EntitySystems;
public sealed partial class ReactionChamberSystem : EntitySystem
{
    [Dependency] readonly IPrototypeManager PrototypeManager = default!;
    [Dependency] UserInterfaceSystem _userInterfaceSystem = default!;
    [Dependency] ItemSlotsSystem _itemSlotsSystem = default!;
    [Dependency]
    SharedSolutionContainerSystem _solutionContainerSystem = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<ReactionChamberComponent, SolutionContainerChangedEvent>(UpdateUiState);
        SubscribeLocalEvent<ReactionChamberComponent, OnTemperatureChangeEvent>(UpdateUiState);
        SubscribeLocalEvent<ReactionChamberComponent, EntInsertedIntoContainerMessage>(UpdateUiState);
        SubscribeLocalEvent<ReactionChamberComponent, EntRemovedFromContainerMessage>(UpdateUiStateNull);
        SubscribeLocalEvent<ReactionChamberComponent, ReactionChamberActiveChangeMessage>(OnActiveChangeMessage);
        SubscribeLocalEvent<ReactionChamberComponent, ReactionChamberTempChangeMessage>(OnTempChangeMessage);
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
                    double deltaJ; // Juoles added to solution
                    float C = solnComp.Solution.GetHeatCapacity(PrototypeManager); //pytoma teploemnist rechowyny (kurs fizyky 8 klass Chlibowska)
                    float solnTemp = solnComp.Solution.GetThermalEnergy(PrototypeManager) / C;
                    float deltaT = comp.Temp - solnTemp;
                    if (solnTemp != comp.Temp)
                    {
                        deltaJ = deltaT * C * frameTime * comp.BaseMultiplier;
                        _solutionContainerSystem.AddThermalEnergy(soln, (float) deltaJ);
                        isAllTempRight = false;
                        if (Math.Abs(deltaJ) <= 0.005)
                        {
                            _solutionContainerSystem.SetTemperature(soln, comp.Temp); // optymizacija included :)
                            isAllTempRight = true;
                            return;
                        }
                    }
                    else
                    {
                        isAllTempRight = true;
                        return;
                    }
                }
        }
    }
    // void OnItemInsert<T>(Entity<ReactionChamberComponent> ent, ref T ev)
    // {
    //     UpdateUiState(ent, ref ev);
    // }
    private void OnActiveChangeMessage(Entity<ReactionChamberComponent> ent, ref ReactionChamberActiveChangeMessage args)
    {
        ent.Comp.Active = args.Active;
        UpdateUiState(ent, ref args);
    }
    private void OnTempChangeMessage(Entity<ReactionChamberComponent> ent, ref ReactionChamberTempChangeMessage args)
    {
        ent.Comp.Temp = float.Clamp(args.Temp, ent.Comp.MinTemp, ent.Comp.MaxTemp);
        UpdateUiState(ent, ref args);
    }
    void UpdateUiStateNull<T>(Entity<ReactionChamberComponent> ent, ref T ev)
    {
        _userInterfaceSystem.SetUiState(ent.Owner, ReactionChamberUiKey.Key, new ReactionChamberBoundUIState());
    }
    void UpdateUiState<T>(Entity<ReactionChamberComponent> ent, ref T ev)
    {
        string? beakerName = null;
        FixedPoint2? beakerVolume = null;
        FixedPoint2? beakerMaxVolume = null;
        List<ReagentQuantity>? reagents = null;
        FixedPoint2? temp = null;
        FixedPoint2? spinBoxTemp = null;
        var beaker = _itemSlotsSystem.GetItemOrNull(ent, "beakerSlot");
        // List<string>? solutions = new List<string>();
        // List<FixedPoint2>? solutionVolumes = new List<FixedPoint2>();
        if (beaker is { Valid: true })
        {
            if (_solutionContainerSystem.TryGetFitsInDispenser(beaker.Value, out var soln, out var solution))
            {
                beakerName = solution.Name;
                beakerVolume = solution.Volume;
                beakerMaxVolume = solution.MaxVolume;
                reagents = solution.Contents;
                spinBoxTemp = float.Clamp(ent.Comp.Temp, ent.Comp.MinTemp, ent.Comp.MaxTemp);
                temp = soln.Value.Comp.Solution.GetThermalEnergy(PrototypeManager) / soln.Value.Comp.Solution.GetHeatCapacity(PrototypeManager);
            }
            _userInterfaceSystem.SetUiState(ent.Owner, ReactionChamberUiKey.Key, new ReactionChamberBoundUIState(new NetEntity(beaker.Value.Id), new BeakerInfo(beakerName, beakerVolume, beakerMaxVolume, reagents, temp, spinBoxTemp)));
        }
    }
}

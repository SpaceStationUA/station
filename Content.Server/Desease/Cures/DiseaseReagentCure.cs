using Content.Shared.Disease;
using Content.Shared.FixedPoint;
using Content.Shared.Chemistry.Reagent;
using Content.Server.Body.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Disease.Cures
{
    /// <summary>
    /// Cures the disease if a certain amount of reagent
    /// is in the host's chemstream.
    /// </summary>
    public sealed partial class DiseaseReagentCure : DiseaseCure
    {
        [DataField("min")]
        public FixedPoint2 Min = 5;
        [DataField("reagent")]
        public string Reagent = "";

        public override bool Cure(DiseaseEffectArgs args)
        {
            if (!args.EntityManager.TryGetComponent<BloodstreamComponent>(args.DiseasedEntity, out var bloodstream))
                return false;

            var quant = FixedPoint2.Zero;
            // if (args.Source != null)
            //     quant = args.Source.GetTotalPrototypeQuantity(Reagent);

            // if (Reagent != null && bloodstream.ChemicalSolution.Value != null)
            // {
            //     quant = bloodstream.ChemicalSolution.GetReagentQuantity(Reagent);
            // }
            return quant >= Min;
        }

        public override string CureText()
        {
            var prototypeMan = IoCManager.Resolve<IPrototypeManager>();
            if (Reagent == null)
                return string.Empty;
            return (Loc.GetString("diagnoser-cure-reagent", ("units", Min), ("reagent", Reagent)));
        }
    }
}

using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared._Impstation.CCVar;

// ReSharper disable once InconsistentNaming
[CVarDefs]
public sealed class ImpCCVars : CVars
{
    public static readonly CVarDef<int> CosmicCultistEntropyValue =
        CVarDef.Create("cosmiccult.cultist_entropy_value", 7, CVar.SERVER, "How much entropy a convert is worth towards the next monument tier");

    public static readonly CVarDef<int> CosmicCultTargetConversionPercent =
        CVarDef.Create("cosmiccult.target_conversion_percent", 40, CVar.SERVER, "How much of the crew the cult is aiming to convert for a tier 3 monument");

    public static readonly CVarDef<int> CosmicCultExtraEntropyForFinale =
        CVarDef.Create<int>("cosmiccult.extra_entropy_for_finale", 20, CVar.SERVER, "How much additional entropy is required to initiate the finale from a tier 3 monument");
}

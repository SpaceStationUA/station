using Robust.Shared.Configuration;


namespace Content.Shared._Pirate;

[CVarDefs]

public sealed class PirateCCVars
{
    public static readonly CVarDef<int>
        VoteRestartMinMinutes = CVarDef.Create("vote.restart_min_duration", 120, CVar.SERVERONLY);
    /// <summary>
    /// Url that the donate button will open.
    /// </summary>
    public static readonly CVarDef<string> SupportUrl =
        CVarDef.Create("support.url", "", CVar.REPLICATED);

    /// <summary>
    /// Whether the arrivals shuttle is enabled.
    /// </summary>
    public static readonly CVarDef<bool> FrontierSpawn =
        CVarDef.Create("frontier.spawn", false);

    /*
     *  Public Transit
     */
    /// <summary>
    /// Whether public transit is enabled.
    /// </summary>
    public static readonly CVarDef<bool> PublicTransit =
        CVarDef.Create("nf14.publictransit.enabled", true, CVar.SERVERONLY);

    /// <summary>
    /// The map to use for the public bus.
    /// </summary>
    public static readonly CVarDef<string> PublicTransitBusMap =
        CVarDef.Create("nf14.publictransit.bus_map", "/Maps/_NF/Shuttles/publicts.yml", CVar.SERVERONLY);

    /// <summary>
    /// The amount of time the bus waits at a station.
    /// </summary>
    public static readonly CVarDef<float> PublicTransitWaitTime =
        CVarDef.Create("nf14.publictransit.wait_time", 150f, CVar.SERVERONLY);

    /// <summary>
    /// The amount of time the flies through FTL space.
    /// </summary>
    public static readonly CVarDef<float> PublicTransitFlyTime =
        CVarDef.Create("nf14.publictransit.fly_time", 145f, CVar.SERVERONLY);
}

using Robust.Shared.Configuration;

namespace Content.Shared.SimpleStation14.CCVar;

[CVarDefs]
public sealed class SimpleStationCCVars
{
    /*
     * Silicons
     */
    #region Silicons
    /// <summary>
    ///     The amount of time between NPC Silicons draining their battery in seconds.
    /// </summary>
    public static readonly CVarDef<float> SiliconNpcUpdateTime =
        CVarDef.Create("silicon.npcupdatetime", 1.5f, CVar.SERVERONLY);
    #endregion Silicons

    #region Tts

    public static readonly CVarDef<float> TTSVolume =
        CVarDef.Create("tts.volume", 0.5f, CVar.CLIENTONLY);

    public static readonly CVarDef<bool> TTSEnabled =
        CVarDef.Create("tts.enabled", false, CVar.SERVERONLY);
    #endregion Tts

}

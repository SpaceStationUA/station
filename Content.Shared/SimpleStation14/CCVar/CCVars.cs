using Robust.Shared.Configuration;

namespace Content.Shared.SimpleStation14.CCVar;

[CVarDefs]
public sealed class SimpleStationCCVars
{
    #region Tts

    public static readonly CVarDef<float> TTSVolume =
        CVarDef.Create("tts.volume", 3f, CVar.CLIENTONLY);

    public static readonly CVarDef<bool> TTSEnabled =
        CVarDef.Create("tts.enabled", false, CVar.SERVERONLY);
    #endregion Tts

}

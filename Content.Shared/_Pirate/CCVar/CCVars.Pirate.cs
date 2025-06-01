using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared._Pirate.CCVar;

/// <summary>
/// Pirate-specific CVars
/// </summary>
[CVarDefs]
public sealed class PirateCVars
{
    /// <summary>
    /// Whether name highlighting is enabled
    /// </summary>
    public static readonly CVarDef<bool> PingNameEnabled =
        CVarDef.Create("pirate.ping_name_enabled", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Whether ping name sounds are enabled
    /// </summary>
    public static readonly CVarDef<bool> PingNameSoundsEnabled =
        CVarDef.Create("pirate.ping_name_sounds_enabled", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Whether to play sound when you mention your own name (for testing)
    /// </summary>
    public static readonly CVarDef<bool> PingNameSelfSoundsEnabled =
        CVarDef.Create("pirate.ping_name_self_sounds_enabled", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Selected ping sound ID
    /// </summary>
    public static readonly CVarDef<string> PingNameSoundId =
        CVarDef.Create("pirate.ping_name_sound_id", "ping1", CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Custom words to highlight and ping on (space-separated)
    /// </summary>
    public static readonly CVarDef<string> PingNameCustomWords =
        CVarDef.Create("pirate.ping_name_custom_words", "", CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Sound cooldown in seconds (0 = no cooldown)
    /// </summary>
    public static readonly CVarDef<float> PingNameSoundCooldown =
        CVarDef.Create("pirate.ping_name_sound_cooldown", 15.0f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Color for highlighted names in hex format (e.g., "#FFFF00" for yellow)
    /// </summary>
    public static readonly CVarDef<string> PingNameColor =
        CVarDef.Create("pirate.ping_name_color", "#FFFF00", CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Volume for ping sounds (0.0 to 1.0)
    /// </summary>
    public static readonly CVarDef<float> PingNameSoundVolume =
        CVarDef.Create("pirate.ping_name_sound_volume", 0.5f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Maximum length of custom words string (server-configurable)
    /// </summary>
    public static readonly CVarDef<int> PingNameCustomWordsMaxLength =
        CVarDef.Create("pirate.ping_name_custom_words_max_length", 250, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Maximum number of custom words allowed (server-configurable)
    /// </summary>
    public static readonly CVarDef<int> PingNameCustomWordsMaxCount =
        CVarDef.Create("pirate.ping_name_custom_words_max_count", 20, CVar.SERVER | CVar.REPLICATED);
}


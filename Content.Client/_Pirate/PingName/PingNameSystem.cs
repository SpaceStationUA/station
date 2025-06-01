using System.Text.RegularExpressions;
using Content.Shared._Pirate.CCVar;
using Content.Shared._Pirate.PingName;
using Content.Shared.Chat;
using Robust.Client.Player;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.GameObjects;
using Robust.Shared.Log;
using Robust.Shared.Maths;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Client._Pirate.PingName;

public sealed class PingNameSystem : SharedPingNameSystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private ISawmill _sawmill = default!;
    private List<string> _playerNameRoots = new();
    private List<string> _customWords = new();
    private List<string> _customWordRoots = new();
    private TimeSpan _lastSoundTime = TimeSpan.Zero;

    public override void Initialize()
    {
        base.Initialize();
        _sawmill = Logger.GetSawmill("pirate.ping_name");

        _player.LocalPlayerAttached += OnLocalPlayerAttached;
        SubscribeLocalEvent<MetaDataComponent, ComponentInit>(OnMetaDataInit);
        SubscribeLocalEvent<MetaDataComponent, EntityRenamedEvent>(OnEntityRenamed);
        _cfg.OnValueChanged(PirateCVars.PingNameCustomWords, OnCustomWordsChanged, true);
    }

    private void OnCustomWordsChanged(string customWords)
    {
        _customWords.Clear();
        _customWordRoots.Clear();

        if (string.IsNullOrWhiteSpace(customWords))
            return;

        var words = customWords.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            var trimmed = word.Trim();
            if (trimmed.Length >= 2)
            {
                var lowerWord = trimmed.ToLowerInvariant();
                _customWords.Add(lowerWord);

                // Generate root for custom word using the same logic as names
                var wordRoot = GetNameRoot(lowerWord);
                if (wordRoot.Length >= 3)
                {
                    _customWordRoots.Add(wordRoot);
                    _sawmill.Debug($"Added custom word '{lowerWord}' with root '{wordRoot}'");
                }
                else
                {
                    // If root is too short, use the word as-is
                    _customWordRoots.Add(lowerWord);
                    _sawmill.Debug($"Added custom word '{lowerWord}' without root (too short)");
                }
            }
        }
    }

    private void OnLocalPlayerAttached(EntityUid entity)
    {
        if (TryComp<MetaDataComponent>(entity, out var meta))
        {
            UpdatePlayerNameRoots(meta.EntityName);
        }
    }

    private void UpdatePlayerNameRoots(string fullName)
    {
        _playerNameRoots.Clear();

        if (string.IsNullOrWhiteSpace(fullName))
            return;

        var nameParts = fullName.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in nameParts)
        {
            var nameRoot = GetNameRoot(part);

            if (nameRoot.Length >= 3)
            {
                _playerNameRoots.Add(nameRoot);
            }
        }
    }

    private void OnMetaDataInit(EntityUid uid, MetaDataComponent component, ComponentInit args)
    {
        if (_player.LocalEntity == uid)
        {
            UpdatePlayerNameRoots(component.EntityName);
        }
    }

    private void OnEntityRenamed(EntityUid uid, MetaDataComponent component, EntityRenamedEvent args)
    {
        if (_player.LocalEntity == uid)
        {
            UpdatePlayerNameRoots(args.NewName);
        }
    }

    public string HighlightNamesInMessage(string message, bool isFromSelf = false)
    {
        if (!_cfg.GetCVar(PirateCVars.PingNameEnabled))
            return message;

        if (_playerNameRoots.Count == 0 && _customWordRoots.Count == 0)
            return message;

        if (IsLoocOrOocMessage(message))
            return message;

        var result = message;
        var foundMatch = false;

        var colorHex = _cfg.GetCVar(PirateCVars.PingNameColor);
        var color = Color.TryFromHex(colorHex) ?? Color.Yellow;
        var font = "bold";

        foreach (var nameRoot in _playerNameRoots)
        {
            var oldResult = result;
            result = HighlightNameInMessage(result, nameRoot, color, font);
            if (oldResult != result)
                foundMatch = true;
        }

        foreach (var customWordRoot in _customWordRoots)
        {
            var oldResult = result;
            result = HighlightNameInMessage(result, customWordRoot, color, font);
            if (oldResult != result)
                foundMatch = true;
        }

        if (foundMatch)
        {
            PlayPingSound(isFromSelf);
        }

        return result;
    }

    private string HighlightNameInMessage(string message, string nameRoot, Color color, string? font = null)
    {
        var colorHex = color.ToHex();
        var escapedRoot = EscapeRegexSpecialChars(nameRoot);
        var namePattern = $@"\b({escapedRoot}\w*)\b";

        string replacement;
        if (font != null)
        {
            replacement = $"[font={font}][color={colorHex}]$1[/color][/font]";
        }
        else
        {
            replacement = $"[color={colorHex}]$1[/color]";
        }

        var bubbleContentPattern = @"(\[BubbleContent\])(.*?)(\[/BubbleContent\])";
        if (Regex.IsMatch(message, bubbleContentPattern))
        {
            var result = Regex.Replace(message, bubbleContentPattern, match =>
            {
                var openTag = match.Groups[1].Value;
                var content = match.Groups[2].Value;
                var closeTag = match.Groups[3].Value;

                var highlightedContent = Regex.Replace(content, namePattern, replacement, RegexOptions.IgnoreCase);

                return openTag + highlightedContent + closeTag;
            }, RegexOptions.Singleline);

            return result;
        }
        else
        {
            var radioPattern = @"(.*?[,:][\s]*[""]*)(.*?)([""]*[\s]*\[/.*?\]*)$";
            var radioMatch = Regex.Match(message, radioPattern, RegexOptions.Singleline);

            if (radioMatch.Success)
            {
                var prefix = radioMatch.Groups[1].Value;
                var messageContent = radioMatch.Groups[2].Value;
                var suffix = radioMatch.Groups[3].Value;

                var highlightedContent = Regex.Replace(messageContent, namePattern, replacement, RegexOptions.IgnoreCase);

                return prefix + highlightedContent + suffix;
            }
            else
            {
                return Regex.Replace(message, namePattern, replacement, RegexOptions.IgnoreCase);
            }
        }
    }

    private bool IsLoocOrOocMessage(string message)
    {
        return message.StartsWith("LOOC:") || message.StartsWith("OOC:");
    }

    private void PlayPingSound(bool isFromSelf = false)
    {
        if (!_cfg.GetCVar(PirateCVars.PingNameSoundsEnabled))
            return;

        if (isFromSelf && !_cfg.GetCVar(PirateCVars.PingNameSelfSoundsEnabled))
            return;

        var cooldownSeconds = _cfg.GetCVar(PirateCVars.PingNameSoundCooldown);
        if (cooldownSeconds > 0)
        {
            var currentTime = _timing.CurTime;
            var timeSinceLastSound = currentTime - _lastSoundTime;

            if (timeSinceLastSound.TotalSeconds < cooldownSeconds)
                return;

            _lastSoundTime = currentTime;
        }

        var soundId = _cfg.GetCVar(PirateCVars.PingNameSoundId);
        var volumeGain = _cfg.GetCVar(PirateCVars.PingNameSoundVolume);

        var soundPath = GetPingSoundPath(soundId);

        try
        {
            // Convert linear gain (0.0-1.0) to decibels for proper volume control
            var volumeDb = SharedAudioSystem.GainToVolume(volumeGain);
            var audioParams = AudioParams.Default.WithVolume(volumeDb);
            _audio.PlayGlobal(soundPath, Filter.Local(), false, audioParams);
        }
        catch (Exception ex)
        {
            _sawmill.Error($"Failed to play ping sound '{soundId}': {ex.Message}");
        }
    }

    public Dictionary<string, string> GetAvailablePingSounds()
    {
        return new Dictionary<string, string>
        {
            { "ping1", "Cargo Ping" },
            { "ping2", "Simple Beep" },
            { "ping3", "Chime" },
            { "ping4", "Ding" },
            { "ping5", "Bell Chime" },
            { "ping6", "Item Beep" },
            { "ping7", "Tech Confirm" },
            { "ping8", "Double Beep" },
            { "ping9", "Desk Bell" },
            { "ping10", "IPC Ding" }
        };
    }

    public void PlayTestPingSound(string soundId)
    {
        var volumeGain = _cfg.GetCVar(PirateCVars.PingNameSoundVolume);

        var soundPath = GetPingSoundPath(soundId);

        try
        {
            // Convert linear gain (0.0-1.0) to decibels for proper volume control
            var volumeDb = SharedAudioSystem.GainToVolume(volumeGain);
            var audioParams = AudioParams.Default.WithVolume(volumeDb);
            _audio.PlayGlobal(soundPath, Filter.Local(), false, audioParams);
        }
        catch (Exception ex)
        {
            _sawmill.Error($"Failed to play test ping sound '{soundId}': {ex.Message}");
        }
    }

    private SoundPathSpecifier GetPingSoundPath(string soundId)
    {
        return soundId switch
        {
            "ping1" => new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg"),
            "ping2" => new SoundPathSpecifier("/Audio/Effects/beep1.ogg"),
            "ping3" => new SoundPathSpecifier("/Audio/Machines/chime.ogg"),
            "ping4" => new SoundPathSpecifier("/Audio/Machines/ding.ogg"),
            "ping5" => new SoundPathSpecifier("/Audio/Effects/chime.ogg"),
            "ping6" => new SoundPathSpecifier("/Audio/Items/beep.ogg"),
            "ping7" => new SoundPathSpecifier("/Audio/Machines/high_tech_confirm.ogg"),
            "ping8" => new SoundPathSpecifier("/Audio/Effects/double_beep.ogg"),
            "ping9" => new SoundPathSpecifier("/Audio/Items/desk_bell_ring.ogg"),
            "ping10" => new SoundPathSpecifier("/Audio/_Pirate/Voice/IPC/ipc_ding_1.ogg"),
            _ => new SoundPathSpecifier("/Audio/Effects/Cargo/ping.ogg")
        };
    }

    private string EscapeRegexSpecialChars(string input)
    {
        return input.Replace("\\", "\\\\")
                   .Replace(".", "\\.")
                   .Replace("+", "\\+")
                   .Replace("*", "\\*")
                   .Replace("?", "\\?")
                   .Replace("^", "\\^")
                   .Replace("$", "\\$")
                   .Replace("(", "\\(")
                   .Replace(")", "\\)")
                   .Replace("[", "\\[")
                   .Replace("]", "\\]")
                   .Replace("{", "\\}")
                   .Replace("}", "\\}")
                   .Replace("|", "\\|");
    }
}



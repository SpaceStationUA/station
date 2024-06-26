﻿using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class LizardAccentSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<LizardAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, LizardAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        // hissss
        message = Regex.Replace(message, "с+", "сс");
        message = Regex.Replace(message, "ш+", "шш");
        // hiSSS
        message = Regex.Replace(message, "С+", "сс");
        message = Regex.Replace(message, "Ш+", "шш");
        // ekssit
        message = Regex.Replace(message, @"(\w)x", "$1kss");
        // ecks
        message = Regex.Replace(message, @"\bx([\-|r|R]|\b)", "ecks$1");
        // eckS
        message = Regex.Replace(message, @"\bX([\-|r|R]|\b)", "ECKS$1");

        args.Message = message;
    }
}

using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class MothAccentSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MothAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, MothAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        message = Regex.Replace(message, "с{1,3}", "зс");
        message = Regex.Replace(message, "ж{1,3}", "зж");
        message = Regex.Replace(message, "ц{1,3}", "зц");
        message = Regex.Replace(message, "з{1,3}", "зз");

        message = Regex.Replace(message, "С{1,3}", "зС");
        message = Regex.Replace(message, "Ж{1,3}", "зЖ");
        message = Regex.Replace(message, "Ц{1,3}", "зЦ");
        message = Regex.Replace(message, "З{1,3}", "ЗЗ");

        // buzzz
        message = Regex.Replace(message, "z{1,3}", "zzz");
        // buZZZ
        message = Regex.Replace(message, "Z{1,3}", "ZZZ");

        args.Message = message;
    }
}

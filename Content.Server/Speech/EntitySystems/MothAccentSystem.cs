using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class MothAccentSystem : EntitySystem
{
    private static readonly Regex RegexLowerBuzz = new Regex("z{1,3}");
    private static readonly Regex RegexUpperBuzz = new Regex("Z{1,3}");

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
        message = RegexLowerBuzz.Replace(message, "zzz");
        // buZZZ
        message = RegexUpperBuzz.Replace(message, "ZZZ");

        args.Message = message;
    }
}

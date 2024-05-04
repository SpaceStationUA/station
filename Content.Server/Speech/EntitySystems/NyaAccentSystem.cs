using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems
{
    public sealed class NyaAccentSystem : EntitySystem
    {
        [Dependency] private readonly IRobustRandom _random = default!;

        private static readonly IReadOnlyList<string> Barks = new List<string>{
            " Ня!", " МЯВ", " мур-мяу", "Няяя", "мяу",
        }.AsReadOnly();

        private static readonly IReadOnlyDictionary<string, string> SpecialWords = new Dictionary<string, string>()
        {
            { "ня", "няв" },
            { "мя", "мяв" },
        };

        public override void Initialize()
        {
            SubscribeLocalEvent<NyaAccentComponent, AccentGetEvent>(OnAccent);
        }

        public string Accentuate(string message)
        {
            foreach (var (word, repl) in SpecialWords)
            {
                message = message.Replace(word, repl);
            }

            return message.Replace("!", _random.Pick(Barks))
                .Replace("l", "r").Replace("L", "R");
        }

        private void OnAccent(EntityUid uid, NyaAccentComponent component, AccentGetEvent args)
        {
            var message = args.Message;
            message = Regex.Replace(message, "р{1,3}", "в");
            message = Regex.Replace(message, "л{1,3}", "в");
            message = Regex.Replace(message, "Р{1,3}", "В");
            message = Regex.Replace(message, "Л{1,3}", "В");
            args.Message = message;

            args.Message = Accentuate(args.Message);
        }
    }
}

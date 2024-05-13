using System.Linq;
using Content.Client._Pirate.TTS;
using Content.Shared._Pirate.TTS;
using Content.Shared.Preferences;
using Robust.Shared.Random;

namespace Content.Client.Preferences.UI;

public sealed partial class HumanoidProfileEditor
{
    private IRobustRandom _random = default!;
    private TTSSystem _ttsSys = default!;
    private List<TTSVoicePrototype> _voiceList = default!;
    private readonly List<string> _sampleText = new()
    {
        "Вітаю станція я телепортував борга прибиральника на станцію СЛАВА НТ.",
        "Так, пані Саро, щодо питання театру. Чи буде інженерія займатись ним?",
        "Так, цей, раз затримали Семуєля, то зелений код?",
        "Він хоче якесь інтерв'ю взяти... Де вас знайти можна?",
        "Семуель Родігрез взламав якоюсь карточкою двері на місток!",
        "Хочу дати належне - газета працює, і доволі непогафно. Мені подобається",
        "Хвала і слава від НТ. Можливо медаль, якщо ще й з виступом для цього подіуму",
        "інженерія, вітаю. Все ж, хтось буде добровольцем у тому, щоб побудувати в театрі подіум?",
        "Клоун, хто у вас що вкрав?",
        "Шефе, в мене буде інтерв'ю брати буде, відійду на 10 хвилин",
        "Наскільки розумію, аномалія зламала з'єднання сингулярності до станції... Саме в тих смесах!"
    };

    private void InitializeVoice()
    {
        _random = IoCManager.Resolve<IRobustRandom>();
        _ttsSys = _entMan.System<TTSSystem>();
        _voiceList = _prototypeManager
            .EnumeratePrototypes<TTSVoicePrototype>()
            .Where(o => o.RoundStart)
            .OrderBy(o => Loc.GetString(o.Name))
            .ToList();

        _voiceButton.OnItemSelected += args =>
        {
            _voiceButton.SelectId(args.Id);
            SetVoice(_voiceList[args.Id].ID);
        };

        _voicePlayButton.OnPressed += _ => { PlayTTS(); };
    }

    private void UpdateTTSVoicesControls()
    {
        if (Profile is null)
            return;

        _voiceButton.Clear();

        var firstVoiceChoiceId = 1;
        for (var i = 0; i < _voiceList.Count; i++)
        {
            var voice = _voiceList[i];
            if (!HumanoidCharacterProfile.CanHaveVoice(voice, Profile.Sex))
                continue;

            var name = Loc.GetString(voice.Name);
            _voiceButton.AddItem(name, i);

            if (firstVoiceChoiceId == 1)
                firstVoiceChoiceId = i;

        }

        var voiceChoiceId = _voiceList.FindIndex(x => x.ID == Profile.Voice);
        if (!_voiceButton.TrySelectId(voiceChoiceId) &&
            _voiceButton.TrySelectId(firstVoiceChoiceId))
        {
            SetVoice(_voiceList[firstVoiceChoiceId].ID);
        }
    }

    private void PlayTTS()
    {
        if (_previewDummy is null || Profile is null)
            return;

        _ttsSys.RequestPreviewTTS(Profile.Voice);
    }
}

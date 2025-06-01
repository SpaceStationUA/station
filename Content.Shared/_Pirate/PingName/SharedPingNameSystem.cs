using Content.Shared.Mind;
using Robust.Shared.GameStates;
using Robust.Shared.Log;
using Robust.Shared.Player;

namespace Content.Shared._Pirate.PingName;

public abstract class SharedPingNameSystem : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _mindSystem = default!;

    protected ISawmill _sawmill = default!;

    public override void Initialize()
    {
        base.Initialize();
        _sawmill = Logger.GetSawmill("pirate.ping_name");
        SubscribeLocalEvent<PingNameComponent, ComponentGetStateAttemptEvent>(OnPingNameGetStateAttempt);
    }

    /// <summary>
    /// Determines if a ping name component should be sent to the client.
    /// </summary>
    private void OnPingNameGetStateAttempt(EntityUid uid, PingNameComponent comp, ref ComponentGetStateAttemptEvent args)
    {
        args.Cancelled = !CanGetState(args.Player, comp);
    }

    /// <summary>
    /// Checks if the player can receive the ping name component state.
    /// Only the owner of the component should receive it.
    /// </summary>
    protected virtual bool CanGetState(ICommonSession? player, PingNameComponent comp)
    {
        if (player?.AttachedEntity is not { Valid: true } playerEntity)
            return false;

        if (!_mindSystem.TryGetMind(player.UserId, out var mindId))
            return false;

        return mindId.Value.Owner == comp.Owner;
    }

    /// <summary>
    /// Generates name roots from name parts using the same logic as the original system.
    /// </summary>
    public static string GetNameRoot(string word)
    {
        if (string.IsNullOrEmpty(word))
            return word;

        var lowerWord = word.ToLowerInvariant();

        // Для коротких слів повертаємо як є
        if (lowerWord.Length <= 3)
            return lowerWord;

        // Українські голосні + англомовна підтримка
        var vowels = "аеєиіїоуюяaeiou";

        // Знаходимо останню приголосну букву
        int lastConsonantIndex = -1;
        for (int i = lowerWord.Length - 1; i >= 0; i--)
        {
            if (!vowels.Contains(lowerWord[i]))
            {
                lastConsonantIndex = i;
                break;
            }
        }

        // Якщо знайшли останню приголосну, обрізаємо все після неї
        if (lastConsonantIndex >= 0)
        {
            var root = lowerWord.Substring(0, lastConsonantIndex + 1);
            // Повертаємо корінь тільки якщо він достатньо довгий
            if (root.Length >= 3)
                return root;
        }

        // Якщо не знайшли приголосну або корінь занадто короткий, повертаємо перші 4-5 символів
        return lowerWord.Length > 4 ? lowerWord.Substring(0, Math.Min(5, lowerWord.Length)) : lowerWord;
    }

    /// <summary>
    /// Updates the name parts and roots for a ping name component.
    /// </summary>
    public void UpdateNameParts(EntityUid uid, string fullName, PingNameComponent? comp = null)
    {
        _sawmill.Debug($"UpdateNameParts called for {uid} with name: '{fullName}'");

        if (!Resolve(uid, ref comp))
        {
            _sawmill.Debug($"Failed to resolve PingNameComponent for {uid}");
            return;
        }

        comp.NameParts.Clear();
        comp.NameRoots.Clear();

        if (string.IsNullOrWhiteSpace(fullName))
        {
            _sawmill.Debug("Full name is null or whitespace");
            return;
        }

        var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        _sawmill.Debug($"Split name into {nameParts.Length} parts: [{string.Join(", ", nameParts)}]");

        foreach (var part in nameParts)
        {
            var nameRoot = GetNameRoot(part);
            _sawmill.Debug($"Part: '{part}' -> Root: '{nameRoot}' (length: {nameRoot.Length})");

            if (nameRoot.Length >= 3) // не хочемо підсвічувати занадто короткі корені
            {
                comp.NameParts.Add(part);
                comp.NameRoots.Add(nameRoot);
                _sawmill.Debug($"Added part '{part}' with root '{nameRoot}'");
            }
            else
            {
                _sawmill.Debug($"Skipped part '{part}' - root too short");
            }
        }

        _sawmill.Debug($"Final name parts: [{string.Join(", ", comp.NameParts)}]");
        _sawmill.Debug($"Final name roots: [{string.Join(", ", comp.NameRoots)}]");

        Dirty(uid, comp);
    }
}

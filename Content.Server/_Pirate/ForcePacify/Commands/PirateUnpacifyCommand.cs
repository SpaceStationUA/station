using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._Pirate.ForcePacify.Commands
{
    [AdminCommand(AdminFlags.Whitelist)]
    public sealed class PirateUnpacifyCommand : IConsoleCommand
    {
        public string Command => "pirate_unpacify";
        public string Description => "Видаляє гравця зі списку pacify.";
        public string Help => "Використання: pirate_unpacify <ім'я_гравця>";

        public async void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length == 0)
            {
                shell.WriteError("Ви повинні вказати ім'я гравця.");
                return;
            }

            var loc = IoCManager.Resolve<IPlayerLocator>();
            var name = args[0].Trim();
            var data = await loc.LookupIdByNameAsync(name);

            if (data != null)
            {
                new PiratePacifyManager().RemoveFromPiratePacify(data.UserId.ToString());
                shell.WriteLine($"Гравець {data.Username} був видалений зі списку pacify.");
            }
            else
            {
                shell.WriteError($"Гравця з ім'ям {name} не знайдено.");
            }
        }
    }
}

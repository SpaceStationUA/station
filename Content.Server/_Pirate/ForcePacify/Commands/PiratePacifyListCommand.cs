using System.Linq;
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._Pirate.ForcePacify.Commands
{
    [AdminCommand(AdminFlags.Whitelist)]
    public sealed class PiratePacifyListCommand : IConsoleCommand
    {
        public string Command => "pirate_pacify_list";
        public string Description => "Показує список усіх гравців у списку pacify.";
        public string Help => "Використання: pirate_pacify_list";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var list = new PiratePacifyManager().GetPiratePacifyList();

            if (list.Any())
            {
                foreach (var entry in list)
                {
                    shell.WriteLine($"Гравець: {entry.UserName} (ID: {entry.UserId}), Дата закінчення: {entry.ExpirationDate}");
                }
            }
            else
            {
                shell.WriteLine("Список pacify порожній.");
            }
        }
    }
}

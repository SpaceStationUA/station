using Content.Server.GameTicking;
using Content.Shared.Administration;
using Content.Shared.GameTicking;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Round)]
    public sealed class ReadyAll : IConsoleCommand
    {
        public string Command => "readyall";
        public string Description => "Готує до гри всіх гравців у лобі, крім спостерігачів";
        public string Help => $"{Command} | ̣{Command} <ready>";
        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var ready = true;

            if (args.Length > 0)
            {
                ready = bool.Parse(args[0]);
            }

            var gameTicker = EntitySystem.Get<GameTicker>();


            if (gameTicker.RunLevel != GameRunLevel.PreRoundLobby)
            {
                shell.WriteLine("This command can only be ran while in the lobby!");
                return;
            }

            gameTicker.ToggleReadyAll(ready);
        }
    }
}

﻿using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.GameTicking.Commands
{
    [AdminCommand(AdminFlags.Round)]
    sealed class EndRoundCommand : IConsoleCommand
    {
        public string Command => "endround";
        public string Description => "Закінчує раунд і переходить сервер у PostRound";
        public string Help => String.Empty;

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var ticker = EntitySystem.Get<GameTicker>();

            if (ticker.RunLevel != GameRunLevel.InRound)
            {
                shell.WriteLine("This can only be executed while the game is in a round.");
                return;
            }

            ticker.EndRound();
        }
    }
}

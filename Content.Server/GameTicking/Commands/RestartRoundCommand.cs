using Content.Server.Administration;
using Content.Server.RoundEnd;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.GameTicking.Commands
{
    [AdminCommand(AdminFlags.Round)]
    public sealed class RestartRoundCommand : IConsoleCommand
    {
        public string Command => "restartround";
        public string Description => "Закінчує поточний раунд і запускає зворотний відлік до наступного лобі";
        public string Help => string.Empty;

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var ticker = EntitySystem.Get<GameTicker>();

            if (ticker.RunLevel != GameRunLevel.InRound)
            {
                shell.WriteLine("This can only be executed while the game is in a round - try restartroundnow");
                return;
            }

            EntitySystem.Get<RoundEndSystem>().EndRound();
        }
    }

    [AdminCommand(AdminFlags.Round)]
    public sealed class RestartRoundNowCommand : IConsoleCommand
    {
        public string Command => "restartroundnow";
        public string Description => "Переводить сервер з PostRound у новий PreRoundLobby";
        public string Help => String.Empty;

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            EntitySystem.Get<GameTicker>().RestartRound();
        }
    }
}

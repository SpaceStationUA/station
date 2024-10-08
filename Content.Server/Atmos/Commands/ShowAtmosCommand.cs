using Content.Server.Administration;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Atmos.Commands
{
    [AdminCommand(AdminFlags.Debug)]
    public sealed class ShowAtmos : IConsoleCommand
    {
        public string Command => "showatmos";
        public string Description => "Перемикає показ накладення налагодження атмосфери";
        public string Help => $"Usage: {Command}";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            if (player == null)
            {
                shell.WriteLine("You must be a player to use this command.");
                return;
            }

            var atmosDebug = EntitySystem.Get<AtmosDebugOverlaySystem>();
            var enabled = atmosDebug.ToggleObserver(player);

            shell.WriteLine(enabled
                ? "Enabled the atmospherics debug overlay."
                : "Disabled the atmospherics debug overlay.");
        }
    }
}

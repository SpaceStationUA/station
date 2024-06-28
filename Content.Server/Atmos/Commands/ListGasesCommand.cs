using Content.Server.Administration;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Atmos.Commands
{
    [AdminCommand(AdminFlags.Debug)]
    public sealed class ListGasesCommand : IConsoleCommand
    {
        public string Command => "listgases";
        public string Description => "Виводить список газів та їх індекси";
        public string Help => "listgases";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var atmosSystem = EntitySystem.Get<AtmosphereSystem>();

            foreach (var gasPrototype in atmosSystem.Gases)
            {
                var gasName = Loc.GetString(gasPrototype.Name);
                shell.WriteLine($"{gasName} ID: {gasPrototype.ID}");
            }
        }
    }

}

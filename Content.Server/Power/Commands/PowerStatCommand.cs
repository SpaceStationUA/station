﻿using Content.Server.Administration;
using Content.Server.Power.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Power.Commands
{
    [AdminCommand(AdminFlags.Debug)]
    public sealed class PowerStatCommand : IConsoleCommand
    {
        public string Command => "powerstat";
        public string Description => "Показує статистику для pow3r";
        public string Help => "Usage: powerstat";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var stats = EntitySystem.Get<PowerNetSystem>().GetStatistics();

            shell.WriteLine($"networks: {stats.CountNetworks}");
            shell.WriteLine($"loads: {stats.CountLoads}");
            shell.WriteLine($"supplies: {stats.CountSupplies}");
            shell.WriteLine($"batteries: {stats.CountBatteries}");
        }
    }
}

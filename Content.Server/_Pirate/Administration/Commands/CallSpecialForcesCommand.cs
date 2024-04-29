using Content.Server.Administration;
using Content.Server.Administration.Logs;
using Content.Server._Pirate.SpecialForces;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Content.Shared.Database;

namespace Content.Server._Pirate.Administration.Commands;

[AdminCommand(AdminFlags.Admin)]
public sealed class CallSpecialForcesCommand : IConsoleCommand
{
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly EntityManager EntityManager = default!;
    public string Command => "callspecforces";

    public string Description => "виклик ert/cburn/deathsquad";

    public string Help => "callspecforces";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if(args.Length != 1){
            shell.WriteLine(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }
        if(!Enum.TryParse<SpecialForcesType>(args[0], true, out var SpecType)){
            shell.WriteLine(Loc.GetString("shell-invalid-entity-id"));
            return;
        }
        var specsys = EntityManager.System<SpecialForcesSystem>();
        if(!EntityManager.System<SpecialForcesSystem>().CallOps(SpecType,shell.Player != null ? shell.Player.Name : "An administrator")){
            shell.WriteLine($"Почекайте ще {specsys.DelayTime} перед викликом наступних!");
        }

        _adminLogger.Add(LogType.AdminMessage, LogImpact.Extreme, $"Admin {(shell.Player != null ? shell.Player.Name : "An administrator")} SpecForcesSystem call {SpecType}");
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args){
        return args.Length switch
        {
            1 => CompletionResult.FromHintOptions(Enum.GetNames<SpecialForcesType>(),
                "Тип команди"),
            _ => CompletionResult.Empty
        };
    }
}
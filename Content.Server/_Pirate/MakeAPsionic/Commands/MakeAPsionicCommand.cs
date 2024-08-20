using Content.Server._Pirate.SpecialForces;
using Content.Server.Administration;
using Content.Server.Administration.Logs;
using Content.Shared.Administration;
using Content.Shared.Ghost;
using Robust.Shared.Console;

namespace Content.Server._Pirate.MakeAPsionic.Commands;

[AdminCommand(AdminFlags.Admin)]
public sealed class MakeAPsionicCommand : IConsoleCommand
{
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly EntityManager EntityManager = default!;

    public string Command => "makepsionic";

    public string Description => "робить вибраного гравця псіоніком";

    public string Help => "makepsionic <uid> <type>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var player = shell.Player;
        if (player != null)
        {
            shell.WriteLine("You cannot use this command from the player.");
            return;
        }

        if (args.Length != 2)
        {
            shell.WriteLine(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (!int.TryParse(args[0], out var intUid))
        {
            shell.WriteError("uid must be a number");
            return;
        }

        var targetNet = new NetEntity(intUid);

        if (!EntityManager.TryGetEntity(targetNet, out var target))
        {
            shell.WriteError("cannot find entity");
            return;
        }

        if (!Enum.TryParse<MakeAPsionicSystem.PsionicType>(args[1], true, out var psionicType))
        {
            shell.WriteLine("invalid psionic type");
            return;
        }

        if (EntityManager.HasComponent<GhostComponent>(target))
        {
            shell.WriteLine("Ghost cannot be made a psionic.");
            return;
        }

        EntityManager.System<MakeAPsionicSystem>().MakePsionic(psionicType, target.Value);
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            2 => CompletionResult.FromHintOptions(Enum.GetNames<MakeAPsionicSystem.PsionicType>(),
                "Тип псіоники"),
            _ => CompletionResult.Empty
        };
    }
}

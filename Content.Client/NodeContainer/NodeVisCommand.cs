using Content.Client.Administration.Managers;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Client.NodeContainer
{
    public sealed class NodeVisCommand : IConsoleCommand
    {
        public string Command => "nodevis";
        public string Description => "Перемикає візуалізацію групи вузлів";
        public string Help => "";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var adminMan = IoCManager.Resolve<IClientAdminManager>();
            if (!adminMan.HasFlag(AdminFlags.Debug))
            {
                shell.WriteError("You need +DEBUG for this command");
                return;
            }

            var sys = EntitySystem.Get<NodeGroupSystem>();
            sys.SetVisEnabled(!sys.VisEnabled);
        }
    }

    public sealed class NodeVisFilterCommand : IConsoleCommand
    {
        public string Command => "nodevisfilter";
        public string Description => "Перемикає показ конкретної групи на nodevis";
        public string Help => "Usage: nodevis [filter]\nOmit filter to list currently masked-off";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var sys = EntitySystem.Get<NodeGroupSystem>();

            if (args.Length == 0)
            {
                foreach (var filtered in sys.Filtered)
                {
                    shell.WriteLine(filtered);
                }
            }
            else
            {
                var filter = args[0];
                if (!sys.Filtered.Add(filter))
                {
                    sys.Filtered.Remove(filter);
                }
            }
        }
    }
}

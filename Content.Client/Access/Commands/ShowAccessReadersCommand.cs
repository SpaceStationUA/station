using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Shared.Console;

namespace Content.Client.Access.Commands;

public sealed class ShowAccessReadersCommand : IConsoleCommand
{
    public string Command => "showaccessreaders";
    public string Description => "Показує всі пристрої зчитування доступу в області перегляду";
    public string Help => $"{Command}";
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var collection = IoCManager.Instance;

        if (collection == null)
            return;

        var overlay = collection.Resolve<IOverlayManager>();

        if (overlay.RemoveOverlay<AccessOverlay>())
        {
            shell.WriteLine($"Set access reader debug overlay to false");
            return;
        }

        var entManager = collection.Resolve<IEntityManager>();
        var cache = collection.Resolve<IResourceCache>();
        var lookup = entManager.System<EntityLookupSystem>();
        var xform = entManager.System<SharedTransformSystem>();

        overlay.AddOverlay(new AccessOverlay(entManager, cache, lookup, xform));
        shell.WriteLine($"Set access reader debug overlay to true");
    }
}

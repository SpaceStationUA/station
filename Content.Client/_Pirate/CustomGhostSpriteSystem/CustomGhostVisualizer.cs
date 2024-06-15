using Content.Client.Ghost;
using Content.Shared.Ghost;
using Content.Shared._Pirate.CustomGhostSystem;
using Robust.Client.GameObjects;

namespace Content.Client._Pirate.CustomGhostSpriteSystem;

public sealed class CustomGhostVisualizer : VisualizerSystem<GhostComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, GhostComponent component, ref AppearanceChangeEvent args)
    {
        base.OnAppearanceChange(uid, component, ref args);

        if(args.Sprite == null) return;

        if (AppearanceSystem.TryGetData<string>(uid, CustomGhostAppearance.Sprite, out var rsiPath, args.Component))
        {
            args.Sprite.LayerSetRSI(0, rsiPath);
        }

        if(AppearanceSystem.TryGetData<float>(uid, CustomGhostAppearance.AlphaOverride, out var alpha, args.Component))
        {
            args.Sprite.Color = args.Sprite.Color.WithAlpha(alpha);
        }
    }
}

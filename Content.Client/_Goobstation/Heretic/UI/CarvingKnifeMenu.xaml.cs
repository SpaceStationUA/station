using Content.Client.UserInterface.Controls;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Prototypes;
using System.Numerics;
using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared.Heretic.Prototypes;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;

namespace Content.Client._Goobstation.Heretic.UI;

public sealed class CarvingKnifeMenu : RadialMenu
{
    [Dependency] private readonly EntityManager _ent = default!;
    [Dependency] private readonly IPrototypeManager _prot = default!;

    private SpriteSystem _sprites;
    private RadialContainer _mainContainer;

    public EntityUid Entity { get; private set; }

    public event Action<ProtoId<RuneCarvingPrototype>>? SendCarvingKnifeSystemMessageAction;

    public CarvingKnifeMenu()
    {
        IoCManager.InjectDependencies(this);

        _sprites = _ent.System<SpriteSystem>();

        _mainContainer = new RadialContainer
        {
            Name = "Main"
        };
        AddChild(_mainContainer);

        HorizontalExpand = true;
        VerticalExpand = true;
        BackButtonStyleClass = "RadialMenuBackButton";
        CloseButtonStyleClass = "RadialMenuCloseButton";
        OnClose += Close;
    }

    public void SetEntity(EntityUid ent)
    {
        Entity = ent;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _mainContainer.RemoveAllChildren();

        if (_ent.TryGetComponent(Entity, out CarvingKnifeComponent? carvingKnife))
        {
            if (carvingKnife.Carvings.Count > 0)
                _mainContainer.Radius = 48f + 24f * MathF.Log(carvingKnife.Carvings.Count);
            else
                 _mainContainer.Radius = 48f;

            foreach (var ammo in carvingKnife.Carvings)
            {
                if (!_prot.TryIndex(ammo, out var prototype))
                    continue;

                var button = new RadialMenuTextureButton
                {
                    SetSize = new Vector2(64, 64),
                    ToolTip = Loc.GetString(prototype.Desc),
                    StyleClasses = { "RadialMenuButton" }
                };

                var texture = new TextureRect
                {
                    VerticalAlignment = VAlignment.Center,
                    HorizontalAlignment = HAlignment.Center,
                    Texture = _sprites.Frame0(prototype.Icon),
                    TextureScale = new Vector2(2f, 2f)
                };

                button.AddChild(texture);
                button.OnButtonUp += _ =>
                {
                    SendCarvingKnifeSystemMessageAction?.Invoke(prototype.ID);
                    Close();
                };
                _mainContainer.AddChild(button);
            }
        }
        else
        {
             _mainContainer.Radius = 48f;
        }
    }
}

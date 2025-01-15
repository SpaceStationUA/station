using Content.Client.Alerts;
using Content.Shared.Alert;
using Content.Client.UserInterface.Systems.Alerts.Controls;
using Content.Shared._Goob.Changeling;
using Content.Shared.StatusIcon.Components;
using Robust.Shared.Prototypes;
using ChangelingComponent = Content.Shared._Goob.Changeling.ChangelingComponent;
using HivemindComponent = Content.Shared._Goob.Changeling.HivemindComponent;


namespace Content.Client.Changeling;

public sealed class ChangelingSystem : SharedChangelingSystem
{

    [Dependency] private readonly IPrototypeManager _prototype = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ChangelingComponent, UpdateAlertSpriteEvent>(OnUpdateAlert);
        SubscribeLocalEvent<ChangelingComponent, GetStatusIconsEvent>(GetChanglingIcon);
    }

    private void OnUpdateAlert(EntityUid uid, ChangelingComponent comp, ref UpdateAlertSpriteEvent args)
    {
        var stateNormalized = 0f;

        // hardcoded because uhh umm i don't know. send help.
        if (args.Alert.AlertKey.AlertType == comp.AlertChemicals)
            stateNormalized = (int) (comp.Chemicals / comp.MaxChemicals * 18);
        else if (args.Alert.AlertKey.AlertType == comp.AlertBiomass)
            stateNormalized = (int) (comp.Biomass / comp.MaxBiomass * 16);
        else
            return;

        var sprite = args.SpriteViewEnt.Comp;
        sprite.LayerSetState(AlertVisualLayers.Base, $"{stateNormalized}");
    }

    private void GetChanglingIcon(Entity<ChangelingComponent> ent, ref GetStatusIconsEvent args)
    {
        if (HasComp<HivemindComponent>(ent) && _prototype.TryIndex(ent.Comp.StatusIcon, out var iconPrototype))
            args.StatusIcons.Add(iconPrototype);
    }
}

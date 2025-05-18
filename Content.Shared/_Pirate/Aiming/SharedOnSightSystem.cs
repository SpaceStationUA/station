using Content.Shared._Pirate.Actions.Events;
using Content.Shared.Alert;
using Robust.Shared.Prototypes;

namespace Content.Shared._Pirate.Aiming;

public sealed partial class SharedOnSightSystem : EntitySystem
{
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<OnSigthComponent, MoveEvent>(OnMove);
        SubscribeLocalEvent<OnSigthComponent, ComponentStartup>(OnStartup);
    }
    private void OnStartup(EntityUid uid, OnSigthComponent component, ComponentStartup args)
    {
        if (_proto.TryIndex<AlertPrototype>("OnSightAlert", out var alertProto))
            _alerts.ShowAlert(uid, alertProto);
    }
    private void OnMove(EntityUid uid, OnSigthComponent component, ref MoveEvent args)
    {
        if (_proto.TryIndex<AlertPrototype>("OnSightAlert", out var alertProto))
            _alerts.ClearAlert(uid, alertProto);
        foreach (var entity in component.AimedAtBy)
        {
            var ev = new OnAimingTargetMoveEvent(uid);
            RaiseLocalEvent(entity, ev);
        }
        RemComp<OnSigthComponent>(uid);
    }
}

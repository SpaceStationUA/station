using Content.Shared._Pirate.ReactionChamber.Components;
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controls;
using Serilog;

namespace Content.Client._Pirate.ReactionChamber.UI;
[UsedImplicitly]
public sealed class ReactionChamberBoundUI : BoundUserInterface
{

    [ViewVariables]
    private ReactionChamberWindow _window;
    public ReactionChamberBoundUI(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        _window = new ReactionChamberWindow();
        _window.FindControl<Button>("ActiveButton").OnPressed += _ => onActiveBtnPressed(_window.Active);
        _window.FindControl<FloatSpinBox>("TempField").OnValueChanged += _ => onTempFieldEntered(_window.FindControl<FloatSpinBox>("TempField").Value);
    }
    protected override void Open()
    {
        base.Open();
        _window.OnClose += Close;

        if (State != null)
        {
            UpdateState(State);
        }

        _window.OpenCentered();
    }
    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        // _window?.u
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _window?.Close();
    }
    private void onActiveBtnPressed(bool active)
    {
        _window.SetActive(!_window.Active);
        SendMessage(new ReactionChamberActiveChangeMessage(active));
    }
    private void onTempFieldEntered(float temp)
    {
        SendMessage(new ReactionChamberTempChangeMessage(temp));
    }
}

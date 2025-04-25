using JetBrains.Annotations;

namespace Content.Client._Pirate.ReactionChamber.UI;
[UsedImplicitly]
public sealed class ReactionChamberBoundUI : BoundUserInterface
{
    private ReactionChamberWindow _window;
    public ReactionChamberBoundUI(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        _window = new ReactionChamberWindow();
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

}

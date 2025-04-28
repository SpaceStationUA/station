
using Robust.Shared.Serialization;

namespace Content.Shared._Pirate.ReactionChamber.Components;
[Serializable]
[NetSerializable]
public sealed class ReactionChamberTempChangeMessage : BoundUserInterfaceMessage
{
    public float Temp;
    public ReactionChamberTempChangeMessage(float temp)
    {
        Temp = temp;
    }
}
[Serializable]
[NetSerializable]
public sealed class ReactionChamberActiveChangeMessage : BoundUserInterfaceMessage
{
    public bool Active;
    public ReactionChamberActiveChangeMessage(bool active)
    {
        Active = active;
    }
}


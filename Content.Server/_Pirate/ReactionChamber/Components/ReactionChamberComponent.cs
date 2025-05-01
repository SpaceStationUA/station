namespace Content.Server._Pirate.ReactionChamber.Components;
[RegisterComponent]
public sealed partial class ReactionChamberComponent : Component
{
    [DataField] public bool Active = false;
    [DataField] public float Temp = 293.15f;
    [DataField] public float BaseMultiplyer = 0.5f;
    [DataField] public float MinTemp = 73.32f;
    [DataField]
    public float MaxTemp = 1000.00f;
}

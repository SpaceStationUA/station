using Robust.Shared.Prototypes;


namespace Content.Shared._Goob.Changeling;

[RegisterComponent]
public sealed partial class ChangelingDartComponent : Component
{
    [DataField(required: true)]
    public ProtoId<_Goob.Changeling.ReagentStingConfigurationPrototype> StingConfiguration;

    [DataField]
    public float ReagentDivisor = 2;
}

using Content.Shared._Goobstation.Weapons.AmmoSelector;
using Robust.Shared.Prototypes;


namespace Content.Shared._Goob.Changeling;

[RegisterComponent]
public sealed partial class ChangelingReagentStingComponent : Component
{
    [DataField(required: true)]
    public ProtoId<_Goob.Changeling.ReagentStingConfigurationPrototype> Configuration;

    [DataField]
    public ProtoId<SelectableAmmoPrototype>? DartGunAmmo;
}

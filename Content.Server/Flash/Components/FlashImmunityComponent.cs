using Content.Server.Changeling;


namespace Content.Server.Flash.Components
{
    [RegisterComponent, Access(typeof(FlashSystem), typeof(ChangelingSystem))]
    public sealed partial class FlashImmunityComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("enabled")]
        public bool Enabled { get; set; } = true;
    }
}

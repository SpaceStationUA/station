using System.ComponentModel.DataAnnotations;
using Robust.Shared.Audio;

namespace Content.Server._Pirate.Power.Components
{.
    [RegisterComponent]
    public sealed partial class IPCChargerComponent : Component
    {
        [DataField] public float ChargeAmount = 72f;
        [DataField] public bool Useable = true;
    }
}

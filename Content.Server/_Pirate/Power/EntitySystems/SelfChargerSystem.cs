using Content.Server._Pirate.Power.Components;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction.Events;
using Content.Shared.Sound.Components;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
namespace Content.Server._Pirate.Power.EntitySystems
{
    public sealed class SelfChargerSystem : EntitySystem
    {
        [Dependency] SharedAudioSystem _audio = default!;
        [Dependency] BatterySystem _battery = default!;
        public override void Initialize()
        {
            SubscribeLocalEvent<IPCChargerComponent, UseInHandEvent>(OnUseInHand);
        }
        private void OnUseInHand(EntityUid uid, IPCChargerComponent component, UseInHandEvent args)
        {
            if (!component.Useable) // check if the charger was already used
                return;
            if (!TryComp(args.User, out ItemSlotsComponent? itemSlot_component)) // checks if player that`ve used charger can hold items(ie. IPC)
                return;
            if (!TryComp(itemSlot_component.Slots["cell_slot"].Item, out BatteryComponent? batteryComponent)) // checks if the player actually have cell inserted
                return;
            _battery.SetCharge(args.User, batteryComponent.CurrentCharge + component.ChargeAmount, batteryComponent);
            component.Useable = false;
            _audio.PlayPvs(component.Sound, uid);
        }
    }
}

using Content.Server._Pirate.Power.Components;
using Content.Server.Construction.Completions;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction.Events;
using Content.Shared.Power;
using Robust.Shared.Audio.Systems;

namespace Content.Server._Pirate.Power.EntitySystems
{
    public sealed class IPCChargerSystem : EntitySystem
    {
        [Dependency] BatterySystem _battery = default!;
        [Dependency] SharedAudioSystem _sound = default!;
        [Dependency] SharedAppearanceSystem _appearence = default!;
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
            var appearance = EntityManager.GetComponentOrNull<AppearanceComponent>(uid);
            _appearence.SetData(uid, PowerChargeVisuals.Active, 0);
            _battery.SetCharge(args.User, batteryComponent.CurrentCharge + component.ChargeAmount, batteryComponent);
            _sound.PlayPvs("/Audio/Effects/PowerSink/charge_fire.ogg", uid);
            component.Useable = false;
        }
    }
}

using Content.Server.DeviceNetwork.Components;
using Content.Server.DeviceNetwork.Systems;

using Content.Shared.Clothing;
using Robust.Server.GameObjects;

namespace Content.Server._Pirate.DeviceNetwork;

public sealed class ActivateCameraOnEquippedSystem : EntitySystem
{
    [Dependency] DeviceNetworkSystem _deviceNetworkSystem = default!;
    [Dependency] AppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ActivateCameraOnEquippedComponent, ClothingGotEquippedEvent>(onEquipped);
        SubscribeLocalEvent<ActivateCameraOnEquippedComponent, ClothingGotUnequippedEvent>(onUnequipped);
    }
    public void onEquipped(EntityUid uid, ActivateCameraOnEquippedComponent component, ClothingGotEquippedEvent args)
    {
        _deviceNetworkSystem.ConnectDevice(uid);
    }
    public void onUnequipped(EntityUid uid, ActivateCameraOnEquippedComponent component, ClothingGotUnequippedEvent args)
    {
        if (!TryComp(uid, out DeviceNetworkComponent? comp))
            return;
        _deviceNetworkSystem.DisconnectDevice(uid, comp);
    }
}

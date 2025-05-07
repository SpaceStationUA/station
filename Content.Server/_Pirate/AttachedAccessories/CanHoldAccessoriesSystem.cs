using Content.Server._Pirate.AttachedAccessories;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;
using Robust.Shared.Serialization.Manager;

namespace Content.Server._Pirate.AttachedAccessories;

public sealed partial class CanHoldAccessoriesSystem : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;
    [Dependency] private readonly ISerializationManager _serialization = default!;
    [Dependency] private readonly IComponentFactory _factory = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CanHoldAccessoriesComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<CanHoldAccessoriesComponent, EntInsertedIntoContainerMessage>(OnAccessoryInserted);
        SubscribeLocalEvent<CanHoldAccessoriesComponent, EntRemovedFromContainerMessage>(OnAccessoryRemoved);
    }
    private void OnInit(EntityUid uid, CanHoldAccessoriesComponent component, ComponentInit args)
    {
        EnsureComp<ItemSlotsComponent>(uid, out var itemSlotsComponent);

        bool hasWhitelistTags = component.WhiteListTags != null && component.WhiteListTags.Count > 0;
        for (int i = 1; i <= component.MaxAccessories; i++)
        {
            var slot = new ItemSlot
            {
                Whitelist = hasWhitelistTags
                ? new EntityWhitelist { Components = ["Accessory"], Tags = component.WhiteListTags, RequireAll = true }
                : new EntityWhitelist { Tags = ["NullAccessory"], RequireAll = true }, //tag that no entity has
                Swap = false,
                EjectOnBreak = true,
                Name = "Accessory"
            };
            _itemSlots.AddItemSlot(uid, $"accessory{i}", slot, itemSlotsComponent);
        }
    }
    private void OnAccessoryInserted(EntityUid uid, CanHoldAccessoriesComponent component, EntInsertedIntoContainerMessage args)
    {
        if (!TryComp<AccessoryComponent>(args.Entity, out var accessoryComponent))
            return;
        if (accessoryComponent.PushComponents is null)
            return;

        foreach (var compName in accessoryComponent.PushComponents)
        {
            if (GetCompInstanceByName(args.Entity, compName, out var compInstance))
            {
                if (compInstance is null)
                    continue;
                if (!HasComp(uid, compInstance.GetType())) //TODO what the fuck is this shit
                {
                    // EntityManager.AddComponent(uid, _serialization.CreateCopy(comp, notNullableOverride: true));
                    EntityManager.AddComponent(uid, compInstance);

                }
                else //TODO still todo this lol
                {
                    var originalComponent = EntityManager.GetComponent(uid, compInstance.GetType());
                    EntityManager.RemoveComponent(uid, compInstance.GetType());
                    EntityManager.AddComponent(uid, compInstance);
                    component.OriginalComponents.Add(originalComponent);
                }
            }
        }
    }
    private void OnAccessoryRemoved(EntityUid uid, CanHoldAccessoriesComponent component, EntRemovedFromContainerMessage args)
    {
        if (!HasComp<AccessoryComponent>(args.Entity))
            return;
        if (component.OriginalComponents is null)
            return;

        List<IComponent?> compsToRemove = new();
        foreach (var comp in component.OriginalComponents)
        {
            if (comp is null)
                continue;
            var compInstance = _serialization.CreateCopy(comp, notNullableOverride: true);
            EntityManager.RemoveComponent(uid, compInstance.GetType());
            EntityManager.AddComponent(uid, compInstance);
            compsToRemove.Add(comp);
        }
        foreach (var comp in compsToRemove)
        {
            component.OriginalComponents.Remove(comp);
        }
    }
    private bool GetCompInstanceByName(EntityUid uid, string compName, out IComponent? compInstance)
    {
        var availability = _factory.GetComponentAvailability(compName);
        if (_factory.TryGetRegistration(compName, out var registration)
            && availability == ComponentAvailability.Available)
        {
            compInstance = _serialization.CreateCopy(EntityManager.GetComponent(uid, registration.Idx), notNullableOverride: true);

            return true;
        }
        compInstance = null;
        return false;
    }
}

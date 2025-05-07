using System.Linq;
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
                if (!HasComp(uid, compInstance.GetType()))
                {
                    EntityManager.AddComponent(uid, compInstance);
                    // component.OriginalComponents.Add(compInstance);
                }
                else
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
        if (!TryComp<AccessoryComponent>(args.Entity, out var accessoryComponent))
            return;
        if (component.OriginalComponents is null)
            return;
        if (accessoryComponent.PushComponents is null)
            return;


        List<IComponent?> compsToRemove = new();
        foreach (var compName in accessoryComponent.PushComponents)
        {
            if (GetCompInstanceByName(uid, compName, out var compInstance))
            {
                Log.Info($"Removing {compName}({compInstance?.GetType().Name}) from {uid}");
                compsToRemove.Add(compInstance);
                if (compInstance is null)
                    continue;
                EntityManager.RemoveComponent(uid, compInstance.GetType());
                if (component.OriginalComponents.AsQueryable().Any(x => x != null && x.GetType() == compInstance.GetType()))
                {
                    var comp = component.OriginalComponents.AsQueryable().First(x => x != null && x.GetType() == compInstance.GetType());
                    var originalCompInstance = _serialization.CreateCopy(comp);
                    if (originalCompInstance is not null)
                        EntityManager.AddComponent(uid, originalCompInstance);
                }

            }
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

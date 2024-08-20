using Robust.Shared.Serialization;

namespace Content.Shared.VendingMachines
{
    [NetSerializable, Serializable]
    public sealed class VendingMachineInterfaceState : BoundUserInterfaceState
    {
        public List<VendingMachineInventoryEntry> Inventory;

        //Pirate banking start
        public double PriceMultiplier;
        public int Credits;

        public VendingMachineInterfaceState(List<VendingMachineInventoryEntry> inventory, double priceMultiplier,
            int credits)
        //Pirate banking end
        {
            Inventory = inventory;
            //Pirate banking start
            PriceMultiplier = priceMultiplier;
            Credits = credits;
            //Pirate banking end
        }
    }

    //Pirate banking start
    [Serializable, NetSerializable]
    public sealed class VendingMachineWithdrawMessage : BoundUserInterfaceMessage
    {
    }
    //Pirate banking end

    [Serializable, NetSerializable]
    public sealed class VendingMachineEjectMessage : BoundUserInterfaceMessage
    {
        public readonly InventoryType Type;
        public readonly string ID;
        public VendingMachineEjectMessage(InventoryType type, string id)
        {
            Type = type;
            ID = id;
        }
    }

    [Serializable, NetSerializable]
    public enum VendingMachineUiKey
    {
        Key,
    }
}

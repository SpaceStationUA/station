using Robust.Shared.Serialization;

namespace Content.Shared.Inventory;

/// <summary>
///     Defines what slot types an item can fit into.
/// </summary>
[Serializable, NetSerializable]
[Flags]
public enum SlotFlags
{
    NONE = 0,
    PREVENTEQUIP = 1 << 0,
    HEAD = 1 << 1,
    EYES = 1 << 2,
    EARS = 1 << 3,
    MASK = 1 << 4,
    OUTERCLOTHING = 1 << 5,
    INNERCLOTHING = 1 << 6,
    NECK = 1 << 7,
    NECK1 = 1 << 8, //PIRATE
    NECK2 = 1 << 9, //PIRATE
    BACK = 1 << 10,
    HEAD1 = 1 << 11, //PIRATE
    HEAD2 = 1 << 12, //PIRATE
    BELT = 1 << 13,
    GLOVES = 1 << 14,
    IDCARD = 1 << 15,
    POCKET = 1 << 16,
    LEGS = 1 << 17,
    FEET = 1 << 18,
    SUITSTORAGE = 1 << 19,
    All = ~NONE,
}

using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Research.Common;

[Serializable, NetSerializable]
public enum ResearchAvailability : byte
{
    Researched,
    Available,
    PrereqsMet,
    Unavailable
}

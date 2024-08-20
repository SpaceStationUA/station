using Content.Server.Abilities.Psionics;
using Content.Server.GameTicking.Rules;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Humanoid;
using Content.Shared.Psionics;
using Robust.Shared.Prototypes;

namespace Content.Server._Pirate.MakeAPsionic;

public sealed class MakeAPsionicSystem : EntitySystem
{
    public enum PsionicType
    {
        TelegnosisPower,
        PsionicRegenerationPower,
        MetapsionicPower,
        PyrokinesisPower,
        NoosphericZapPower,
        MindSwapPower,
        MassSleepPower,
        DispelPower,
    }

    [Dependency] private readonly PsionicAbilitiesSystem _psionicAbilitiesSystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public void MakePsionic(PsionicType psionicType, EntityUid entity)
    {
        EnsureComp<PsionicComponent>(entity);
        if (!_prototypeManager.TryIndex<PsionicPowerPrototype>(psionicType.ToString(), out var power))
            return;

        _psionicAbilitiesSystem.InitializePsionicPower(entity, power);
    }
}

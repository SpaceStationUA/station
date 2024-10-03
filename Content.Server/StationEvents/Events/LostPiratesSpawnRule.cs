using Robust.Server.GameObjects;
using Robust.Server.Maps;
using Robust.Shared.Map;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Components;
using Content.Server.StationEvents.Components;

namespace Content.Server.StationEvents.Events;

public sealed class LostPiratesSpawnRule : StationEventSystem<LostPiratesSpawnRuleComponent>
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly MapLoaderSystem _map = default!;
    [Dependency] private readonly GameTicker _gameTicker = default!;

    protected override void Started(EntityUid uid, LostPiratesSpawnRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        var shuttleMap = _mapManager.CreateMap();
        var options = new MapLoadOptions
        {
            LoadMap = true,
        };

        _map.TryLoad(shuttleMap, component.LostPiratesShuttlePath, out _, options);

        // var lostpiratesEntity = _gameTicker.AddGameRule(component.GameRuleProto);
        // component.AdditionalRule = lostpiratesEntity;
        // _gameTicker.StartGameRule(lostpiratesEntity);
    }

    protected override void Ended(EntityUid uid, LostPiratesSpawnRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.Ended(uid, component, gameRule, args);

        if (component.AdditionalRule != null)
            GameTicker.EndGameRule(component.AdditionalRule.Value);
    }
}

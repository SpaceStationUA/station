using System.Net.Http;
using System.Threading.Tasks;
using Content.Server.Station.Systems;
using Content.Server.StationRecords;
using Content.Server.StationRecords.Systems;
using Content.Shared.StationRecords;
using Robust.Server.ServerStatus;
using Robust.Shared.Enums;


namespace Content.Server.Administration;

public sealed partial class ServerApi
{
    private static readonly HashSet<string> SecurityJobNames = new()
    {
        "headofsecurity",
        "blueshield",
        "blueshieldofficer",
        "warden",
        "detective",
        "securityofficer",
        "securitycadet",
        "seniorofficer"
    };

    [Dependency] private readonly IEntityManager _entMan = default!;

    static ServerApi()
    {
        RegisterEndpoint(api => api.RegisterSecurityHandlers());
    }

    private void RegisterSecurityHandlers()
    {
        RegisterHandler(HttpMethod.Get, "/admin/security_online_players", GetSecurityOnlinePlayers);
    }

    private async Task GetSecurityOnlinePlayers(IStatusHandlerContext context)
    {
        var count = await RunOnMainThread(() =>
        {
            int securityCount = 0;

            var entMan = IoCManager.Resolve<IEntityManager>();
            var stationSystem = entMan.System<StationSystem>();
            var stationRecordsSystem = entMan.System<StationRecordsSystem>();

            foreach (var player in _playerManager.Sessions)
            {
                if (player.Status != SessionStatus.InGame)
                    continue;

                var attachedEntity = player.AttachedEntity;
                if (attachedEntity == null || entMan.IsQueuedForDeletion(attachedEntity.Value))
                    continue;

                var station = stationSystem.GetOwningStation(attachedEntity.Value);
                if (station == null)
                    continue;

                if (!entMan.TryGetComponent(station.Value, out StationRecordsComponent? stationRecords))
                    continue;

                var playerName = entMan.GetComponent<MetaDataComponent>(attachedEntity.Value).EntityName;

                var recordId = stationRecordsSystem.GetRecordByName(station.Value, playerName);
                if (recordId == null)
                    continue;

                var key = new StationRecordKey(recordId.Value, station.Value);

                if (!stationRecordsSystem.TryGetRecord<GeneralStationRecord>(key, out var record, stationRecords))
                    continue;

                var jobTitle = record.JobPrototype;
                if (string.IsNullOrEmpty(jobTitle))
                    continue;

                if (SecurityJobNames.Contains(jobTitle.ToLower()))
                {
                    securityCount++;
                }
            }

            return securityCount;
        });

        var response = new SecurityOnlinePlayersResponse
        {
            SecurityOnlinePlayers = count
        };

        await context.RespondJsonAsync(response);
    }

    private sealed class SecurityOnlinePlayersResponse
    {
        public int SecurityOnlinePlayers { get; init; }
    }
}

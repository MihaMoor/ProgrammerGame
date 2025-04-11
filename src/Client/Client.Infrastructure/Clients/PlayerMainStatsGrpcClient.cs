using Google.Protobuf.WellKnownTypes;
using Shared.GrpcContracts;

namespace Client.Infrastructure.Clients;

public class PlayerMainStatsGrpcClient(
    string adress,
    PlayerMainStatsService.PlayerMainStatsServiceClient client)
    : GrpcClient<PlayerMainStatsService.PlayerMainStatsServiceClient>(adress, client)
{
    public async Task<PlayerMainStatsDto> GetAsync(
        Action<PlayerMainStatsDto> handler,
        CancellationToken cancellationToken)
    {
        PlayerMainStatsDto? dto = null;
        try
        {
            using var call = Client.GetAsync(new Empty(), cancellationToken: cancellationToken);
            while (await call.ResponseStream.MoveNext(cancellationToken))
            {
                dto = call.ResponseStream.Current;
                handler(dto);
            }
        }
        catch (Exception ex)
        {
            // Logging or send message to server for registration bug
            //logger.LogCritical(ex, ex.Message);
        }

        return
            dto ?? new()
            {
                Health = 100,
                Hunger = 100,
                Money = 99.99,
                Mood = 100
            };
    }
}

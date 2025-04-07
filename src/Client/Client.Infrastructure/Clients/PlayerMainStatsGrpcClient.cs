using Google.Protobuf.WellKnownTypes;
using Shared.GrpcContracts;

namespace Client.Infrastructure.Clients;

public class PlayerMainStatsGrpcClient : GrpcClient<PlayerMainStatsService.PlayerMainStatsServiceClient>
{
    public PlayerMainStatsGrpcClient(
        string adress,
        PlayerMainStatsService.PlayerMainStatsServiceClient client)
        : base(adress, client)
    { }

    public async Task<PlayerMainStatsDto> GetAsync()
    {
        PlayerMainStatsDto? dto = null;
        try
        {
            dto = await Client.GetAsync(new Empty());
        }
        catch (Exception)
        {
            // Logging or send message to server for registration bug
        }

        return
            dto ??= new()
            {
                Health = 100,
                Hunger = 100,
                Money = 99.99,
                Mood = 100
            };
    }
}

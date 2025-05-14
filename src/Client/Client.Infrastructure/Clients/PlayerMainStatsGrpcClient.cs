using Server.Module.Player.GrpcContracts;

namespace Client.Infrastructure.Clients;

public class PlayerGrpcClient(string adress, PlayerService.PlayerServiceClient client)
    : GrpcClient<PlayerService.PlayerServiceClient>(adress, client)
{
    public async Task<PlayerDto> Get(Action<PlayerDto> handler, CancellationToken cancellationToken)
    {
        PlayerDto? dto = null;
        try
        {
            UUID uUID = new() { Id = Guid.NewGuid().ToString() };
            using var call = Client.Get(uUID, cancellationToken: cancellationToken);
            while (await call.ResponseStream.MoveNext(cancellationToken))
            {
                dto = call.ResponseStream.Current;
                handler(dto);
            }
        }
        catch (Exception)
        {
            // Logging or send message to server for registration bug
            //logger.LogCritical(ex, ex.Message);
        }

        return dto
            ?? new()
            {
                Name = "Unknown",
                Health = 100,
                Hunger = 100,
                Mood = 100,
                PocketMoney = 0.01,
            };
    }
}

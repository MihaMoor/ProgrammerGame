using Server.Module.Player.GrpcContracts;

namespace Client.Infrastructure.Clients;

public class PlayerGrpcClient(string adress, PlayerService.PlayerServiceClient client)
    : GrpcClient<PlayerService.PlayerServiceClient>(adress, client)
{
    /// <summary>
    /// Streams player data from the gRPC service, invoking the provided handler for each received <see cref="PlayerDto"/>.
    /// </summary>
    /// <param name="handler">An action to process each <see cref="PlayerDto"/> received from the stream.</param>
    /// <param name="cancellationToken">Token to cancel the streaming operation.</param>
    /// <returns>The last <see cref="PlayerDto"/> received from the stream, or a default player if none are received.</returns>
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

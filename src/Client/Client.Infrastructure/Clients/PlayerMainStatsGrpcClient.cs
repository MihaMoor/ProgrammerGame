using Server.Module.Player.GrpcContracts;

namespace Client.Infrastructure.Clients;

public class PlayerGrpcClient(string adress, PlayerService.PlayerServiceClient client)
    : GrpcClient<PlayerService.PlayerServiceClient>(adress, client)
{
    /// <summary>
    /// Retrieves player data from the gRPC service and invokes the provided handler with the result.
    /// Returns a default player object if the retrieval fails.
    /// </summary>
    /// <param name="handler">An action to process the retrieved <see cref="PlayerDto"/>.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The retrieved <see cref="PlayerDto"/>, or a default player if an error occurs.</returns>
    public PlayerDto Get(Action<PlayerDto> handler, CancellationToken cancellationToken)
    {
        PlayerDto? dto = null;
        try
        {
            UUID uUID = new() { Id = Guid.NewGuid().ToString() };
            dto = Client.Get(uUID, cancellationToken: cancellationToken);

            handler(dto);
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

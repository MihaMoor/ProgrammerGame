using Grpc.Core;
using Server.Module.Player.GrpcContracts.V1;

namespace Client.Infrastructure.Clients;

public class PlayerGrpcClient(string adress, PlayerService.PlayerServiceClient client)
    : GrpcClient<PlayerService.PlayerServiceClient>(adress, client)
{
    /// <summary>
    /// Retrieves a player profile from the gRPC service and invokes the provided handler with the result.
    /// </summary>
    /// <param name="handler">A delegate to process the retrieved <see cref="PlayerDto"/>.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The retrieved <see cref="PlayerDto"/>, or a default player profile if retrieval fails.</returns>
    public PlayerDto Get(Action<PlayerDto> handler, CancellationToken cancellationToken)
    {
        PlayerDto? dto = null;
        try
        {
            UUID uUID = new() { Id = Guid.NewGuid().ToString() };
            dto = Client.Get(uUID, cancellationToken: cancellationToken);

            handler?.Invoke(dto);
        }
        catch (RpcException rpcEx)
        {
            // Logging or send message to server for registration bug
            Console.WriteLine($"gRPC ошибка: {rpcEx.StatusCode} - {rpcEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
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

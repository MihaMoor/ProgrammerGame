using Grpc.Core;
using Server.Module.Player.GrpcContracts;

namespace Client.Infrastructure.Clients;

public class PlayerGrpcClient(string adress, PlayerService.PlayerServiceClient client)
    : GrpcClient<PlayerService.PlayerServiceClient>(adress, client)
{
    public PlayerDto Get(Action<PlayerDto> handler, CancellationToken cancellationToken)
    {
        PlayerDto? dto = null;
        try
        {
            UUID uUID = new() { Id = Guid.NewGuid().ToString() };
            dto = Client.Get(uUID, cancellationToken: cancellationToken);

            handler(dto);
        }
        catch (RpcException rpcEx)
        {
            // Logging or send message to server for registration bug
            Console.WriteLine($"gRPC ошибка: {rpcEx.StatusCode} - {rpcEx.Message}");
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

using Grpc.Core;
using Server.Module.Player.GrpcContracts.V1;

namespace Client.Infrastructure.Clients;

public class PlayerGrpcClient(string adress, PlayerService.PlayerServiceClient client)
    : GrpcClient<PlayerService.PlayerServiceClient>(adress, client)
{
    public PlayerDto Get(Action<PlayerDto> handler, CancellationToken cancellationToken)
    {
        PlayerDto? dto = null;
        try
        {
            UUID uUID = new() { PlayerId = "01973485-47c7-7cbc-834f-ec060fbb5e75" };
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
                PocketMoney = new()
                {
                    Units = 0,
                    Nanos = 10_000_000 // 0.01 = 10 миллионов нано-единиц
                },
            };
    }
}

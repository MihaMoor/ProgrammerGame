using Shared.GrpcContracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Server.Api.Services;

public class PlayerMainStatsServiceServer : PlayerMainStatsService.PlayerMainStatsServiceBase
{
    public override async Task GetAsync(
        Empty empty,
        IServerStreamWriter<PlayerMainStatsDto> responceStream,
        ServerCallContext context)
    {
        PlayerMainStatsDto playerMainStatsDto = new()
        {
            Health = 100,
            Hunger = 100,
            Money = 100,
            Mood = 100
        };

        var random = new Random();

        while(!context.CancellationToken.IsCancellationRequested)
        {
            playerMainStatsDto.Health = (uint)random.Next(0, 100);
            playerMainStatsDto.Hunger = (uint)random.Next(0, 100);
            playerMainStatsDto.Money = random.NextDouble() * 100;
            playerMainStatsDto.Mood = (uint)random.Next(0, 100);

            await responceStream.WriteAsync(playerMainStatsDto);
            await Task.Delay(1000);
        }
    }
}

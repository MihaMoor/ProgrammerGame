using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Shared.GrpcContracts;

namespace Server.Api.Services;

public class PlayerGrpcService(ILogger<PlayerGrpcService> logger) : PlayerService.PlayerServiceBase
{
    public override async Task GetAsync(
        Empty empty,
        IServerStreamWriter<PlayerDto> responceStream,
        ServerCallContext context
    )
    {
        PlayerDto playerDto = new()
        {
            Name = "Test",
            Health = 100,
            Hunger = 100,
            Money = 100,
            Mood = 100,
            PocketMoney = 25,
        };

        var random = new Random();

        while (!context.CancellationToken.IsCancellationRequested)
        {
            playerDto.Health = (uint)random.Next(0, 100);
            playerDto.Hunger = (uint)random.Next(0, 100);
            playerDto.Money = random.NextDouble() * 100;
            playerDto.Mood = (uint)random.Next(0, 100);

            logger.LogInformation(
                $"Player stats.\nHealth: {playerDto.Health}\nHunger: {playerDto.Hunger}\nMoney: {playerDto.Money}\nMood: {playerDto.Mood}"
            );

            await responceStream.WriteAsync(playerDto);
            await Task.Delay(1000);
        }
    }
}

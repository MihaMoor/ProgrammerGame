using Grpc.Core;
using Microsoft.Extensions.Logging;
using Module.Player.GrpcContracts;

namespace Module.Player.Api;

internal class GetPlayerGrpcService(ILogger<GetPlayerGrpcService> logger)
    : PlayerService.PlayerServiceBase
{
    public override async Task GetAsync(UUID request, IServerStreamWriter<PlayerDto> responseStream, ServerCallContext context)
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

            await responseStream.WriteAsync(playerDto);
            await Task.Delay(1000);
        }
    }

    public override async Task<PlayerDto> GetByIdAsync(UUID request, ServerCallContext context)
    {
        var command = new GetPlayerQuery
        {
            Id = new System.Guid(request.Id),
        };

        return await base.GetByIdAsync(request, context);
    }
}
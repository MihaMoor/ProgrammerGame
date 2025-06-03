using Microsoft.Extensions.Logging;
using Moq;
using Server.Module.Player.Api;
using Server.Module.Player.Application;
using Server.Module.Player.GrpcContracts.V1;
using Server.Shared.Cqrs;

namespace Server.Module.Player.IntegrationTests;

[Trait("Category", "Integration")]
public partial class PlayerTests
{
    //[Theory(Timeout = 1000)]
    //[MemberData(nameof(CreatePlayerData))]
    public async Task Получение_игрока(
        UUID id,
        PlayerDto expected)
    {
        // Arrange
        Mock<ILogger<GetPlayerGrpcService>> mockLogger = new();
        Mock<IPlayerRepository> playerRepositoryMock = new();
        IQueryHandler<GetPlayerQuery, Domain.Player> queryHandler = new GetPlayerQueryHandler(playerRepositoryMock.Object);

        GetPlayerGrpcService service = new(mockLogger.Object, queryHandler);
        PlayerDto player = await service.Get(id, null!);

        Assert.Equal(expected.Name, player.Name);
        Assert.Equal(expected.Health, player.Health);
        Assert.Equal(expected.Hunger, player.Hunger);
        Assert.Equal(expected.Mood, player.Mood);
        Assert.Equal(expected.PocketMoney, player.PocketMoney);
    }
}

public partial class PlayerTests
{
    public static TheoryData<UUID, PlayerDto> CreatePlayerData => new()
    {
        {
            new()
            {
                PlayerId = Guid.NewGuid().ToString(),
            },
            new()
            {
                PlayerId = Guid.NewGuid().ToString(),
                Name = "Test1",
                Health = 100,
                Hunger = 100,
                Mood = 100,
                PocketMoney =
                {
                    Units = 99,
                    Nanos = (int)(0.99 * 1_000_000_000)
                },
            }
        }
    };
}

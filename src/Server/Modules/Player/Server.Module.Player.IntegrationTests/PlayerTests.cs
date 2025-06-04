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
    private static readonly Mock<ILogger<PlayerGrpcService>> mockLogger = new();
    private static readonly Mock<IPlayerRepository> playerRepositoryMock = new();
    private static readonly Mock<IPlayerChangeNotifier> playerChangeNotifierMock = new();

    [Theory(Timeout = 5_000)]
    [MemberData(nameof(CreatePlayerData))]
    public async Task Получение_игрока(
        UUID id,
        PlayerDto expected)
    {
        // Arrange
        IQueryHandler<GetPlayerQuery, Domain.Player> queryHandler = new GetPlayerQueryHandler(playerRepositoryMock.Object);
        PlayerGrpcService service = new(mockLogger.Object, queryHandler, null!);
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
    private static readonly Guid playerId = Guid.CreateVersion7();

    public static TheoryData<UUID, PlayerDto> CreatePlayerData => new()
    {
        {
            new()
            {
                PlayerId = playerId.ToString(),
            },
            new()
            {
                PlayerId = playerId.ToString(),
                Name = "Test1",
                Health = 100,
                Hunger = 100,
                Mood = 100,
                PocketMoney =
                {
                    Units = 99,
                    Nanos = 99_000_000
                },
            }
        }
    };
}

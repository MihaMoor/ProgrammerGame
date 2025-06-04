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
    private readonly Mock<ILogger<PlayerGrpcService>> mockLogger = new();
    private readonly Mock<IPlayerRepository> playerRepositoryMock = new();
    private readonly Mock<IPlayerChangeNotifier> playerChangeNotifierMock = new();

    [Theory]
    [MemberData(nameof(CreatePlayerData))]
    public async Task Получение_игрока(
        UUID id,
        Domain.Player seed,
        PlayerDto expected)
    {
        // Arrange
        playerRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(seed);

        IQueryHandler<GetPlayerQuery, Domain.Player> queryHandler = new GetPlayerQueryHandler(playerRepositoryMock.Object);
        PlayerGrpcService service = new(mockLogger.Object, queryHandler, null!);

        PlayerDto player = await service.Get(id, null!);

        Assert.NotNull(player);
        Assert.Equal(expected.PlayerId, player.PlayerId);
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

    public static TheoryData<UUID, Domain.Player, PlayerDto> CreatePlayerData => new()
    {
        {
            new()
            {
                PlayerId = playerId.ToString(),
            },
            Domain.Player.CreatePlayer(
                playerId : playerId,
                name : "Test1",
                health : 100,
                hunger : 100,
                mood : 100,
                pocketMoney : 99.99m,
                isAlive: true
            )
            .Value,
            new()
            {
                PlayerId = playerId.ToString(),
                Name = "Test1",
                Health = 100,
                Hunger = 100,
                Mood = 100,
                PocketMoney = new()
                {
                    Units = 99,
                    Nanos = 990_000_000,
                    CurrencyCode = "RUB"
                },
            }
        }
    };
}

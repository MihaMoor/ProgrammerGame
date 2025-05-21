using Server.Module.Player.Application;
using Server.Shared.Cqrs;
using Server.Shared.Errors;

namespace Server.Module.Player.IntegrationTests;

[Trait("Category", "Integration")]
public partial class PlayerTests(IQueryHandler<GetPlayerQuery, Domain.Player> queryHandler)
{
    [Theory(Timeout = 1000)]
    [MemberData(nameof(CreatePlayerData))]
    public async Task Получение_игрока(
        GetPlayerQuery query,
        Domain.Player expected)
    {
        Result<Domain.Player> playerResult = await queryHandler.Handle(query);

        Assert.True(playerResult.IsSuccess);

        Domain.Player player = playerResult.Value;

        Assert.Equal(expected.Name, player.Name);
        Assert.Equal(expected.Health, player.Health);
        Assert.Equal(expected.Hunger, player.Hunger);
        Assert.Equal(expected.Mood, player.Mood);
        Assert.Equal(expected.PocketMoney, player.PocketMoney);
    }
}

public partial class PlayerTests
{
    public static TheoryData<GetPlayerQuery, Domain.Player> CreatePlayerData => new()
    {
        {
            new(Guid.NewGuid()),
            Domain.Player.CreatePlayer("Test1").Value
        }
    };
}

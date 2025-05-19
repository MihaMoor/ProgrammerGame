using Server.Module.Player.Domain;
using Server.Shared.Errors;

namespace Server.Module.Player.UnitTests;

[Trait("Category", "Unit")]
public partial class PlayerTests
{
    [Theory]
    [MemberData(nameof(CreateMainStatsData))]
    public void Создания_основных_характеристик(string name, bool expected, Error? error)
    {
        Result<MainStats> actual = MainStats.CreatePlayer(name);

        Assert.Equal(expected, actual.IsSuccess);
        if (actual.IsFailure)
        {
            Assert.Equal(error, actual.Error);
        }
    }
}

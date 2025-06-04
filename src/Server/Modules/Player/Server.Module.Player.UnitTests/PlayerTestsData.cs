using Server.Module.Player.Domain;
using Server.Shared.Errors;

namespace Server.Module.Player.UnitTests;

public partial class PlayerTests
{
    public static TheoryData<string, bool, Error?> CreateMainStatsData =>
        new()
        {
            { "test1", true, null },
            { "Игрок с русским именем", true, null },
            { "Player with very long name that might cause issues if there are restrictions", true, null },
            { "", false, PlayerError.NameIsEmpty() },
            { " ", false, PlayerError.NameIsEmpty() },
        };
}

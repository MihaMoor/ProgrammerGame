using Server.Module.Player.Domain;
using Server.Shared.Errors;

namespace Server.Module.Player.UnitTests;

public partial class PlayerTests
{
    public static TheoryData<string, bool, Error?> CreateMainStatsData =>
        new()
        {
            { "test1", true, null },
            { "", false, PlayerError.NameIsEmpty() },
            { " ", false, PlayerError.NameIsEmpty() },
        };
}

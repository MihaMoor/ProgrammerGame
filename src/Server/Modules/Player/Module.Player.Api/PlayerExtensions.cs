using Server.Module.Player.Domain;
using Server.Module.Player.GrpcContracts;

namespace Server.Module.Player.Api;

internal static class PlayerExtensions
{
    public static PlayerDto ToViewModel(this MainStats stats) =>
        new()
        {
            Name = stats.Name,
            Health = stats.Health,
            Hunger = stats.Hunger,
            PocketMoney = stats.PocketMoney,
            Mood = stats.Mood,
        };
}

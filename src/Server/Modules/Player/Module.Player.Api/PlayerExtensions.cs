using Server.Module.Player.Domain;
using Server.Module.Player.GrpcContracts.Player.V1;

namespace Server.Module.Player.Api;

internal static class PlayerExtensions
{
    public static PlayerDto ToViewModel(this Domain.Player stats) =>
        new()
        {
            Name = stats.Name,
            Health = stats.Health,
            Hunger = stats.Hunger,
            PocketMoney = (double)stats.PocketMoney,
            Mood = stats.Mood,
        };
}

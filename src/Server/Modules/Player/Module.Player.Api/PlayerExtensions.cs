using Server.Module.Player.GrpcContracts.V1;

namespace Server.Module.Player.Api;

internal static class PlayerExtensions
{
    /// <summary>
        /// Converts a <see cref="Domain.Player"/> instance to a <see cref="PlayerDto"/> for API responses.
        /// </summary>
        /// <param name="stats">The player domain object to convert.</param>
        /// <returns>A <see cref="PlayerDto"/> containing the mapped player data.</returns>
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

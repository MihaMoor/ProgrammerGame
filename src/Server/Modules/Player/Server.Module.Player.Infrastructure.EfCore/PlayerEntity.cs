using Server.Shared.Errors;

namespace Server.Module.Player.Infrastructure.EfCore;

public sealed record PlayerEntity(
    Guid PlayerId,
    string Name,
    uint Health,
    uint Hunger,
    uint Mood,
    decimal PocketMoney,
    bool IsAlive);

public static class PlayerEntityExtensions
{
    /// <summary>
    /// Converts a <see cref="PlayerEntity"/> to a domain <see cref="Domain.Player"/>.
    /// </summary>
    /// <param name="playerEntity">The player entity to convert.</param>
    /// <returns>The corresponding domain player.</returns>
    /// <exception cref="Exception">Thrown if the player cannot be created, indicating the player is dead.</exception>
    public static Domain.Player ToPlayerDomain(this PlayerEntity playerEntity)
    {
        Result<Domain.Player> playerResult = Domain.Player.CreatePlayer(playerEntity.Name);

        if (playerResult.IsFailure)
        {
            throw new Exception($"Player with Id='{playerEntity.PlayerId}' is dead!");
        }

        return playerResult.Value;
    }

    /// <summary>
            /// Converts a domain <c>Player</c> object to a <c>PlayerEntity</c> for persistence.
            /// </summary>
            /// <param name="player">The domain player to convert.</param>
            /// <returns>A <c>PlayerEntity</c> representing the given domain player.</returns>
            public static PlayerEntity ToPlayerEntity(this Domain.Player player)
        => new(
            player.PlayerId,
            player.Name,
            player.Health,
            player.Hunger,
            player.Mood,
            player.PocketMoney,
            player.IsAlive);
}
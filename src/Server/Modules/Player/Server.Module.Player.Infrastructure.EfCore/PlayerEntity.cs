namespace Server.Module.Player.Infrastructure.EfCore;

public sealed record PlayerEntity(
    Guid PlayerId,
    string Name,
    int Health,
    int Hunger,
    int Mood,
    decimal PocketMoney,
    bool IsAlive);

public static class PlayerEntityExtensions
{
    /// <summary>
    /// Converts a <see cref="PlayerEntity"/> to a <see cref="Domain.Player"/> domain object.
    /// </summary>
    /// <returns>The corresponding <see cref="Domain.Player"/> instance.</returns>
    /// <exception cref="Exception">Thrown if the domain player cannot be created from the entity's data.</exception>
    public static Domain.Player ToPlayerDomain(this PlayerEntity playerEntity)
    {
        var playerResult = Domain.Player.CreatePlayer(
            playerEntity.PlayerId,
            playerEntity.Name,
            playerEntity.Health,
            playerEntity.Hunger,
            playerEntity.Mood,
            playerEntity.PocketMoney,
            playerEntity.IsAlive);

        if (playerResult.IsFailure)
        {
            throw new Exception($"Failed to create player with Id='{playerEntity.PlayerId}': {playerResult.Error}");
        }

        return playerResult.Value;
    }

    /// <summary>
            /// Converts a domain player object to a <see cref="PlayerEntity"/> for persistence.
            /// </summary>
            /// <param name="player">The domain player to convert.</param>
            /// <returns>A <see cref="PlayerEntity"/> representing the given domain player.</returns>
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
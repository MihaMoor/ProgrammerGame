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
    /// Преобразует объект <see cref="PlayerEntity"/> в доменный объект <see cref="Domain.Player"/>.
    /// </summary>
    /// <returns>Соответствующий экземпляр <see cref="Domain.Player"/>.</returns>
    /// <exception cref="Exception">Выбрасывается, если из данных сущности невозможно создать доменного игрока.</exception>
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
    /// Преобразует доменный объект игрока в <see cref="PlayerEntity"/> для сохранения в базе данных.
    /// </summary>
    /// <param name="player">Доменный объект игрока, который необходимо преобразовать.</param>
    /// <returns>Объект <see cref="PlayerEntity"/>, представляющий указанный доменный объект игрока.</returns>
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
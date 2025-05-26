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
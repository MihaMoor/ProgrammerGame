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
    public static Domain.Player ToPlayerDomain(this PlayerEntity playerEntity)
    {
        Result<Domain.Player> playerResult = Domain.Player.CreatePlayer(playerEntity.Name);

        if (playerResult.IsFailure)
        {
            throw new Exception($"Player with Id='{playerEntity.PlayerId}' is dead!");
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
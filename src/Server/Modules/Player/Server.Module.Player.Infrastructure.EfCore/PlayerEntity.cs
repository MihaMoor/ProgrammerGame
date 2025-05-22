namespace Server.Module.Player.Infrastructure.EfCore;

public sealed record PlayerEntity(
    Guid PlayerId,
    string Name,
    uint Health,
    uint Hunger,
    uint Mood,
    decimal PocketMoney);

using RealTimePrototype.Domain.Aggregates;

namespace RealTimePrototype.API.Dtos.Players;

public record PlayerDto(int Id, float Satiety);

public static class PlayerExtensions
{
    public static PlayerDto FromDomain(this Player player)
        => new(player.Id, player.Satiety);
}

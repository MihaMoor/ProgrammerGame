using RealTimePrototype.Domain.Aggregates;

namespace RealTimePrototype.API.Dtos.Players;

public record PlayerDto(int Id, float Satiety, float Mood);

public static class PlayerDtoExtensions
{
    public static PlayerDto FromDomain(this Player player)
        => new(player.Id, player.Satiety, player.Mood);
}

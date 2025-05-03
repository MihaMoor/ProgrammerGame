using RealTimePrototype.Domain.Aggregates;

namespace RealTimePrototype.API.Dtos.Players;

public record CreatePlayerDto(int Id, float Satiety, float Mood);

public static class CreatePlayerDtoExtensions
{
    public static Player ToPlayerDomain(this CreatePlayerDto createPlayer)
        => new()
        {
            Id = createPlayer.Id,
            Satiety = createPlayer.Satiety,
            Mood = createPlayer.Mood
        };
}

using RealTimePrototype.Domain.Aggregates;

namespace RealTimePrototype.API.Dtos.Players;

public record UpdatePlayerDto(int Id, float Satiety, float Mood);

public static class UpdatePlayerDtoExtensions
{
    public static Player ToPlayerDomain(this UpdatePlayerDto updatePlayer)
        => new()
        {
            Id = updatePlayer.Id,
            Satiety = updatePlayer.Satiety,
            Mood = updatePlayer.Mood
        };
}

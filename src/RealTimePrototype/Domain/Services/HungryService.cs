using RealTimePrototype.Domain.Abstractions;
using RealTimePrototype.Domain.Aggregates;

namespace RealTimePrototype.Domain.Services;

public class HungryService(IPlayerRepository playerRepository, float unitPerTick = 5)
{
    private readonly HashSet<int> _activePlayerIds = [];

    public void Activate(int playerId)
        => _activePlayerIds.Add(playerId);

    public void Deactivate(int playerId)
        => _activePlayerIds.Remove(playerId);

    public void Hungry()
    {
        foreach (var id in _activePlayerIds)
        {
            Player? player = playerRepository.GetById(id);

            if (player == null)
                continue;

            player.Satiety -= (player.Mood >= 50)
                ? unitPerTick
                : unitPerTick * 2;

            playerRepository.Update(player);
        }
    }
}

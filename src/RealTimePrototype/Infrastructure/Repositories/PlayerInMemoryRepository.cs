using RealTimePrototype.Domain.Abstractions;
using RealTimePrototype.Domain.Aggregates;
using System.Collections.Concurrent;

namespace RealTimePrototype.Infrastructure.Repositories;

public sealed class PlayerInMemoryRepository : IPlayerRepository
{
    private static readonly ConcurrentDictionary<int, Player> s_database = [];

    public bool Create(Player player)
    {
        if (s_database.ContainsKey(player.Id))
            return false;

        s_database[player.Id] = player;
        return true;
    }

    public Player? GetById(int id)
    {
        s_database.TryGetValue(id, out Player? player);
        return player;
    }

    public bool Update(Player player)
    {
        if (!s_database.ContainsKey(player.Id))
            return false;

        s_database[player.Id] = player;
        return true;
    }
}

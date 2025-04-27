using RealTimePrototype.Domain.Aggregates;

namespace RealTimePrototype.Domain.Abstractions;

public interface IPlayerRepository
{
    bool Create(Player player);

    Player? GetById(int id);

    bool Update(Player player);
}

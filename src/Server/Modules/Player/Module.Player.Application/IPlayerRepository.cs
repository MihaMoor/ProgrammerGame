using Server.Module.Player.Domain;

namespace Server.Module.Player.Application;

public interface IPlayerRepository
{
    /// <summary>
    /// Получение игрока по Id.
    /// </summary>
    /// <param name="id">Id игрока</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Игрока</returns>
    Task<Domain.Player?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}

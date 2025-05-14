using Server.Module.Player.Domain;

namespace Server.Module.Player.Infrastructure;

public interface IMainStatsRepository
{
    /// <summary>
    /// Получение игрока по Id.
    /// </summary>
    /// <param name="id">Id игрока</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Игрока</returns>
    Task<MainStats?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}

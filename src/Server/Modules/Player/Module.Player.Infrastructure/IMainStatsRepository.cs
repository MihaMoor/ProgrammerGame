using Server.Module.Player.Domain;

namespace Server.Module.Player.Infrastructure;

public interface IMainStatsRepository
{
    /// <summary>
    /// Получение игрока по Id.
    /// </summary>
    /// <param name="id">Id игрока</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <summary>
/// Asynchronously retrieves the main statistics for a player by their unique identifier.
/// </summary>
/// <param name="id">The unique identifier of the player.</param>
/// <param name="cancellationToken">Optional token to cancel the operation.</param>
/// <returns>The player's main statistics if found; otherwise, null.</returns>
    Task<MainStats?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}

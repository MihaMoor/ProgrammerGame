namespace Server.Module.Player.Application;

public interface IPlayerRepository
{
    /// <summary>
    /// Асинхронно извлекает сущность игрока по её уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор игрока для извлечения.</param>
    /// <param name="cancellationToken">Необязательный токен для отмены операции.</param>
    /// <returns>Сущность игрока, если найдена; в противном случае, null.</returns>
    Task<Domain.Player?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}

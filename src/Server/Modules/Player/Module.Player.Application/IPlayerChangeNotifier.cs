namespace Server.Module.Player.Application;

public interface IPlayerChangeNotifier
{
    /// <summary>
    /// Подписывается на изменения для конкретного игрока и регистрирует обработчик, который вызывается при изменении данных игрока.
    /// </summary>
    /// <param name="playerId">Уникальный идентификатор игрока, за изменениями которого нужно следить.</param>
    /// <param name="handler">Асинхронный обработчик, который выполняется при обновлении данных игрока.</param>
    /// <returns>Объект <see cref="IDisposable"/>, который можно использовать для отмены подписки.</returns>
    IDisposable Subscribe(Guid playerId, Func<Domain.Player, Task> handler);

    /// <summary>
    /// Уведомляет о том, что основные статистические данные указанного игрока изменились.
    /// </summary>
    /// <param name="player">Игрок, основные статистические данные которого были обновлены.</param>
    /// <returns>Задача, представляющая асинхронную операцию уведомления.</returns>
    Task OnMainStatsChanged(Domain.Player player);
}

using Server.Module.Player.Domain;

namespace Server.Module.Player.Infrastructure;

public interface IMainStatsChangeNotifier
{
    // Подписка на изменения MainStats по ID, возвращает IDisposable для отмены подписки
    IDisposable Subscribe(Guid mainStatsId, Func<MainStats, Task> handler);
}

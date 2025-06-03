namespace Server.Module.Player.Application;

public interface IPlayerChangeNotifier
{
    // Подписка на изменения Player по ID, возвращает IDisposable для отмены подписки
    IDisposable Subscribe(Guid mainStatsId, Func<Domain.Player, Task> handler);
    Task OnMainStatsChanged(Domain.Player stats);
}

namespace Server.Module.Player.Application;

public interface IPlayerChangeNotifier
{
    /// <summary>
    /// ������������� �� ��������� ��� ����������� ������ � ������������ ����������, ������� ���������� ��� ��������� ������ ������.
    /// </summary>
    /// <param name="playerId">���������� ������������� ������, �� ����������� �������� ����� �������.</param>
    /// <param name="handler">����������� ����������, ������� ����������� ��� ���������� ������ ������.</param>
    /// <returns>������ <see cref="IDisposable"/>, ������� ����� ������������ ��� ������ ��������.</returns>
    IDisposable Subscribe(Guid playerId, Func<Domain.Player, Task> handler);

    /// <summary>
    /// ���������� � ���, ��� �������� �������������� ������ ���������� ������ ����������.
    /// </summary>
    /// <param name="player">�����, �������� �������������� ������ �������� ���� ���������.</param>
    /// <returns>������, �������������� ����������� �������� �����������.</returns>
    Task OnMainStatsChanged(Domain.Player player);
}

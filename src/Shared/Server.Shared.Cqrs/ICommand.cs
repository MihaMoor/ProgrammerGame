using Server.Shared.Errors;

namespace Server.Shared.Cqrs;

public interface ICommand { }

public interface ICommand<out TResponse> : ICommand { }

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// ������������ ��������� ������� ����������.
    /// </summary>
    /// <param name="command">�������, ������� ���������� ����������.</param>
    /// <param name="cancellationToken">����� ��� ������������ �������� ������.</param>
    /// <returns>������, �������������� ����������� ��������, ���������� ��������� ���������� �������.</returns>
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// ������������ �������, ��������� �����, �������� �� ���������� � ���������� ������.
    /// </summary>
    /// <param name="command">������� ��� ���������.</param>
    /// <param name="cancellationToken">����� ��� ������������ �������� ������.</param>
    /// <returns>������, �������������� ����������� ��������, ���������� ��������� � ������ ������.</returns>
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

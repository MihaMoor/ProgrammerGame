using Server.Shared.Errors;

namespace Server.Shared.Cqrs;

public interface IQuery<TResponse> { }

public interface IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// ������������ ��������� ������ ���������� � ���������� ���������.
    /// </summary>
    /// <param name="query">������ ��� ���������.</param>
    /// <param name="cancellationToken">�������������� ����� ��� ������ ��������.</param>
    /// <returns>������, �������������� ����������� ��������, ���������� ��������� �������.</returns>
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}

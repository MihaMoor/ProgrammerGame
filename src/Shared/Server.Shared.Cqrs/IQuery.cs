using Server.Shared.Errors;

namespace Server.Shared.Cqrs;

public interface IQuery<TResponse> { }

public interface IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// Обрабатывает указанный запрос асинхронно и возвращает результат.
    /// </summary>
    /// <param name="query">Запрос для обработки.</param>
    /// <param name="cancellationToken">Необязательный токен для отмены операции.</param>
    /// <returns>Задача, представляющая асинхронную операцию, содержащая результат запроса.</returns>
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}

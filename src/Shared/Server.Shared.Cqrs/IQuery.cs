using Server.Shared.Errors;

namespace Server.Shared.Cqrs;

public interface IQuery<TResponse> { }

public interface IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
/// Handles the specified query asynchronously and returns the result.
/// </summary>
/// <param name="query">The query to process.</param>
/// <param name="cancellationToken">Optional token to cancel the operation.</param>
/// <returns>A task representing the asynchronous operation, containing the result of the query.</returns>
Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}

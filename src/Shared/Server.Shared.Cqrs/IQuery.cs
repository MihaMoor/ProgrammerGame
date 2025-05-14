using Server.Shared.Results;

namespace Server.Shared.Cqrs;

public interface IQuery<TResponse> { }

public interface IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
/// Processes the specified query asynchronously and returns the result.
/// </summary>
/// <param name="query">The query to handle.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>A task representing the asynchronous operation, containing the result of the query.</returns>
Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}

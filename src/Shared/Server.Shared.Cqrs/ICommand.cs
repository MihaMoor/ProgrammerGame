using Server.Shared.Results;

namespace Server.Shared.Cqrs;

public interface ICommand { }

public interface ICommand<TResponse> { }

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    /// <summary>
/// Processes a command asynchronously and returns the result of the operation.
/// </summary>
/// <param name="command">The command to be handled.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>A task representing the asynchronous operation, containing the result status.</returns>
Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
/// Handles a command asynchronously and returns a result containing a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <param name="command">The command to process.</param>
/// <param name="cancellationToken">Optional token to cancel the operation.</param>
/// <returns>A task representing the asynchronous operation, containing the result and response.</returns>
Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

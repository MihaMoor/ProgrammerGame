using Server.Shared.Errors;

namespace Server.Shared.Cqrs;

public interface ICommand { }

public interface ICommand<out TResponse> : ICommand { }

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    /// <summary>
/// Processes the specified command asynchronously and returns the result.
/// </summary>
/// <param name="command">The command to be handled.</param>
/// <param name="cancellationToken">Token to observe for cancellation requests.</param>
/// <returns>A task representing the asynchronous operation, containing the result of command execution.</returns>
Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
/// Handles a command asynchronously and returns a result containing a response.
/// </summary>
/// <param name="command">The command to process.</param>
/// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
/// <returns>A task representing the asynchronous operation, containing the result with the response.</returns>
Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

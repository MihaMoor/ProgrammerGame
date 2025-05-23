using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using Server.Module.Player.GrpcContracts.V1;
using Server.Shared.Cqrs;
using Server.Shared.Errors;

namespace Server.Module.Player.Api;

internal sealed class SubscribePlayerGrpcService(
    ILogger<SubscribePlayerGrpcService> _logger,
    IQueryHandler<SubscribePlayer, IAsyncEnumerable<Domain.Player>> _subscribeHandler
) : PlayerService.PlayerServiceBase
{
    /// <summary>
    /// Handles a subscription request for player updates and streams real-time player data to the client.
    /// </summary>
    /// <param name="request">The subscription request containing the player ID.</param>
    /// <param name="responseStream">The server stream writer used to send player updates to the client.</param>
    /// <param name="context">The gRPC call context.</param>
    /// <remarks>
    /// Validates the player ID, executes the subscription query, and streams player updates as they become available.
    /// Throws a <see cref="RpcException"/> with appropriate status codes for invalid input, query failures, or streaming errors.
    /// </remarks>
    public override async Task Subscribe(
        UUID request,
        IServerStreamWriter<PlayerDto> responseStream,
        ServerCallContext context
    )
    {
        _logger.LogInformation("Subscribe request received for player {PlayerId}", request.Id);

        if (!Guid.TryParse(request.Id, out Guid playerId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid player Id"));

        SubscribePlayer query = new(playerId);

        Result<IAsyncEnumerable<Domain.Player>> result = await _subscribeHandler.Handle(
            query,
            context.CancellationToken
        );

        if (result.IsFailure)
        {
            _logger.LogError("Failed to subscribe to player stats: {Error}", result.Error);

            StatusCode status = result.Error.Code.ToStatusCode();
            throw new RpcException(new Status(status, result.Error.ToString()));
        }

        try
        {
            // Обрабатываем поток обновлений и отправляем их клиенту
            await foreach (
                Domain.Player? stats in result.Value.WithCancellation(context.CancellationToken)
            )
            {
                await responseStream.WriteAsync(stats.ToViewModel());
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Error while streaming player stats updates");
            throw new RpcException(
                new Status(StatusCode.Internal, "Streaming error: " + ex.Message)
            );
        }
    }
}

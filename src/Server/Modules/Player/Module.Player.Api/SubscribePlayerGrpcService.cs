using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using Server.Module.Player.Domain;
using Server.Module.Player.GrpcContracts;
using Server.Shared.Cqrs;

namespace Server.Module.Player.Api;

internal sealed class SubscribePlayerGrpcService(
    ILogger<SubscribePlayerGrpcService> _logger,
    IQueryHandler<SubscribeMainStats, IAsyncEnumerable<MainStats>> _subscribeHandler
) : PlayerService.PlayerServiceBase
{
    /// <summary>
    /// Streams real-time updates of a player's main statistics to the client in response to a subscription request.
    /// </summary>
    /// <param name="request">The subscription request containing the player's UUID.</param>
    /// <param name="responseStream">The server stream writer used to send <see cref="PlayerDto"/> updates to the client.</param>
    /// <param name="context">The gRPC call context, supporting cancellation and status propagation.</param>
    /// <remarks>
    /// Validates the player ID, initiates a subscription to the player's main statistics, and streams updates as they become available.
    /// Throws a gRPC <see cref="RpcException"/> with <see cref="StatusCode.InvalidArgument"/> if the player ID is invalid,
    /// or with <see cref="StatusCode.Internal"/> if the subscription fails.
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

        SubscribeMainStats query = new(playerId);

        Shared.Results.Result<IAsyncEnumerable<MainStats>> result = await _subscribeHandler.Handle(
            query,
            context.CancellationToken
        );

        if (result.IsFailure)
        {
            _logger.LogError("Failed to subscribe to player stats: {Error}", result.Error);
            throw new RpcException(
                new Status(StatusCode.Internal, $"Subscribe failed: {result.Error}")
            );
        }

        try
        {
            // Обрабатываем поток обновлений и отправляем их клиенту
            await foreach (
                MainStats? stats in result.Value.WithCancellation(context.CancellationToken)
            )
            {
                await responseStream.WriteAsync(stats.ToViewModel());
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Error while streaming player stats updates");
        }
    }
}

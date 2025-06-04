using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using Server.Module.Player.GrpcContracts.V1;
using Server.Shared.Cqrs;
using Server.Shared.Errors;

namespace Server.Module.Player.Api;

public class PlayerGrpcService(
    ILogger<PlayerGrpcService> _logger,
    IQueryHandler<GetPlayerQuery, Domain.Player> _handler,
    IQueryHandler<SubscribePlayer, IAsyncEnumerable<Domain.Player>> _subscribeHandler
) : PlayerService.PlayerServiceBase
{
    /// <summary>
    /// Обрабатывает gRPC-запрос получения игрока.
    /// </summary>
    /// <param name="request">UUID запроса.</param>
    /// <returns>Информация об игроке.</returns>
    /// <exception cref="RpcException">
    /// Выбрасывается со статусом <see cref="StatusCode.InvalidArgument"/>
    /// при null или пустом Id у <paramref name="request"/>.
    /// <summary>
    /// Retrieves player information for the specified player ID.
    /// </summary>
    /// <param name="request">The UUID containing the player ID to retrieve.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>The player data as a <see cref="PlayerDto"/>.</returns>
    /// <exception cref="RpcException">
    /// Thrown if the player cannot be found or if validation fails, with an appropriate gRPC status code.
    /// </exception>
    public override async Task<PlayerDto> Get(UUID request, ServerCallContext context)
    {
        UUID validatedRequest = Validation.Validate(request, _logger);

        GetPlayerQuery query = new(new(validatedRequest.PlayerId));


        Result<Domain.Player> result = await _handler.Handle(query);

        if (result.IsFailure)
        {
            _logger.LogError(result.Error.ToString());
            StatusCode statusCode = result.Error.Code.ToStatusCode();
            throw new RpcException(new Status(statusCode, result.Error.ToString()));
        }

        return result.Value.ToViewModel();
    }

    /// <summary>
    /// Streams real-time updates of a player's data to the client as they become available.
    /// </summary>
    /// <param name="request">The request containing the player ID to subscribe to.</param>
    /// <param name="responseStream">The server stream used to send player updates to the client.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <remarks>
    /// Throws an <see cref="RpcException"/> with <see cref="StatusCode.InvalidArgument"/> if the player ID is invalid,
    /// or with an appropriate status code if subscription fails or a streaming error occurs.
    /// </remarks>
    public override async Task Subscribe(
        UUID request,
        IServerStreamWriter<PlayerDto> responseStream,
        ServerCallContext context
    )
    {
        _logger.LogInformation("Subscribe request received for player {PlayerId}", request.PlayerId);

        if (!Guid.TryParse(request.PlayerId, out Guid playerId))
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

    /// <summary>
    /// Handles a request to create a new player by logging the command and delegating to the base implementation.
    /// </summary>
    /// <param name="request">The command containing player creation details.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A task representing the asynchronous operation, with the created player's data.</returns>
    public override Task<PlayerDto> Create(CreatePlayerCommand request, ServerCallContext context)
    {
        _logger.LogInformation(request.ToString());
        return base.Create(request, context);
    }
}

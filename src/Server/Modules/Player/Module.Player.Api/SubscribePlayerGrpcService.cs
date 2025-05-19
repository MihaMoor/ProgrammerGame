using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using Server.Module.Player.Domain;
using Server.Module.Player.GrpcContracts.Player.V1;
using Server.Shared.Cqrs;
using Server.Shared.Errors;

namespace Server.Module.Player.Api;

internal sealed class SubscribePlayerGrpcService(
    ILogger<SubscribePlayerGrpcService> _logger,
    IQueryHandler<SubscribeMainStats, IAsyncEnumerable<MainStats>> _subscribeHandler
) : PlayerService.PlayerServiceBase
{
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

        Result<IAsyncEnumerable<MainStats>> result = await _subscribeHandler.Handle(
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
                MainStats? stats in result.Value.WithCancellation(context.CancellationToken)
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

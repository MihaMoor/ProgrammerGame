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
    /// Получает информацию об игроке для указанного идентификатора игрока.
    /// </summary>
    /// <param name="request">UUID, содержащий идентификатор игрока для получения.</param>
    /// <param name="context">Контекст вызова сервера gRPC.</param>
    /// <returns>Данные игрока в виде <see cref="PlayerDto"/>.</returns>
    /// <exception cref="RpcException">
    /// Возникает, если игрок не найден или если проверка не удалась, с соответствующим кодом состояния gRPC.
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
    /// Потоковая передача обновлений данных игрока клиенту в режиме реального времени по мере их появления.
    /// </summary>
    /// <param name="request">Запрос, содержащий идентификатор игрока для подписки.</param>
    /// <param name="responseStream">Серверный поток, используемый для отправки обновлений игрока клиенту.</param>
    /// <param name="context">Контекст вызова сервера gRPC.</param>
    /// <remarks>
    /// Генерирует исключение <see cref="RpcException"/> с <see cref="StatusCode.InvalidArgument"/>, если идентификатор игрока недействителен,
    /// или с соответствующим кодом состояния, если подписка не удалась или произошла ошибка потоковой передачи.
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
    /// Обрабатывает запрос на создание нового игрока, регистрируя команду и передавая её на базовую реализацию.
    /// </summary>
    /// <param name="request">Команда, содержащая детали создания игрока.</param>
    /// <param name="context">Контекст вызова сервера gRPC.</param>
    /// <returns>Задача, представляющая асинхронную операцию, с данными созданного игрока.</returns>
    public override Task<PlayerDto> Create(CreatePlayerCommand request, ServerCallContext context)
    {
        _logger.LogInformation(request.ToString());
        return base.Create(request, context);
    }
}

using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.Application;
using Server.Module.Player.GrpcContracts.V1;
using Server.Shared.Cqrs;
using Server.Shared.Errors;

namespace Server.Module.Player.Api;

public class GetPlayerGrpcService(
    ILogger<GetPlayerGrpcService> _logger,
    IQueryHandler<GetPlayerQuery, Domain.Player> _handler
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
    /// Handles a gRPC request to retrieve player information by UUID.
    /// </summary>
    /// <param name="request">The UUID of the player to retrieve.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A <see cref="PlayerDto"/> representing the requested player.</returns>
    /// <exception cref="RpcException">
    /// Thrown if the request UUID is invalid or if the player cannot be retrieved due to a domain error.
    /// </exception>
    public override async Task<PlayerDto> Get(UUID request, ServerCallContext context)
    {
        UUID validatedRequest = Validation.Validate(request, _logger);

        GetPlayerQuery query = new(new(validatedRequest.Id));

        Result<Domain.Player> result = await _handler.Handle(query);

        if (result.IsFailure)
        {
            _logger.LogError(result.Error.ToString());
            StatusCode statusCode = result.Error.Code.ToStatusCode();
            throw new RpcException(new Status(statusCode, result.Error.ToString()));
        }

        return result.Value.ToViewModel();
    }
}

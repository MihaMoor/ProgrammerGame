using Grpc.Core;
using Microsoft.Extensions.Logging;
using Module.Player.Api;
using Server.Module.Player.Application;
using Server.Module.Player.Domain;
using Server.Module.Player.GrpcContracts;
using Server.Shared.Cqrs;
using Server.Shared.Results;

namespace Server.Module.Player.Api;

internal class GetPlayerGrpcService(
    ILogger<GetPlayerGrpcService> _logger,
    IQueryHandler<GetMainStatsQuery, MainStats> _handler
) : PlayerService.PlayerServiceBase
{
    public override async Task<PlayerDto> Get(UUID request, ServerCallContext context)
    {
        UUID validatedRequest = Validation.Validate(request, _logger);

        GetMainStatsQuery query = new(new(validatedRequest.Id));

        Result<MainStats> result = await _handler.Handle(query);

        if (result.IsFailure)
        {
            _logger.LogError(result.Error.ToString());
            StatusCode statusCode = result.Error.Code.ToStatusCode();
            throw new RpcException(new Status(statusCode, result.Error.ToString()));
        }

        return result.Value.ToViewModel();
    }
}

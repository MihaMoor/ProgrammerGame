using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.GrpcContracts.Player.V1;

namespace Server.Module.Player.Api;

internal class CreatePlayerGrpcService(ILogger<CreatePlayerGrpcService> logger)
    : CreatePlayerService.CreatePlayerServiceBase
{
    public override Task<PlayerDto> Create(CreatePlayerCommand request, ServerCallContext context)
    {
        logger.LogInformation(request.ToString());
        return base.Create(request, context);
    }
}

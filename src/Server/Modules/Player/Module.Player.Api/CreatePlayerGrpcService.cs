using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.GrpcContracts;

namespace Server.Module.Player.Api;

internal class CreatePlayerGrpcService(ILogger<CreatePlayerGrpcService> logger)
    : CreatePlayerService.CreatePlayerServiceBase
{
    /// <summary>
    /// Logs the incoming player creation request and delegates the creation operation to the base service.
    /// </summary>
    /// <param name="request">The player creation command containing player details.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A task representing the asynchronous operation, with the created player's data.</returns>
    public override Task<PlayerDto> Create(CreatePlayerCommand request, ServerCallContext context)
    {
        logger.LogInformation(request.ToString());
        return base.Create(request, context);
    }
}

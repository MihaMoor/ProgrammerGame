using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.GrpcContracts.V1;

namespace Server.Module.Player.Api;

internal class CreatePlayerGrpcService(ILogger<CreatePlayerGrpcService> logger)
    : CreatePlayerService.CreatePlayerServiceBase
{
    /// <summary>
    /// Handles a gRPC request to create a new player by delegating to the base implementation.
    /// </summary>
    /// <param name="request">The command containing player creation details.</param>
    /// <param name="context">The gRPC server call context.</param>
    /// <returns>A task representing the asynchronous operation, with the created player's data.</returns>
    public override Task<PlayerDto> Create(CreatePlayerCommand request, ServerCallContext context)
    {
        logger.LogInformation(request.ToString());
        return base.Create(request, context);
    }
}

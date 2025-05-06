using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Module.Player.Api;

internal class CreatePlayerGrpcService(ILogger<CreatePlayerGrpcService> logger)
: CreatePlayerService.CreatePlayerServiceBase
{
    public override Task<PlayerDto> CreateAsync(CreatePlayerCommand request, ServerCallContext context)
    {

        return base.CreateAsync(request, context);
    }
}

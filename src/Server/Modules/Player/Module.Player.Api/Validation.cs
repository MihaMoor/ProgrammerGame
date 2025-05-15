using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.GrpcContracts;

namespace Server.Module.Player.Api;

internal class Validation
{
    internal static UUID Validate(UUID request, ILogger logger)
    {
        if (request == null)
        {
            string message = "Request cannot be null";
            logger.LogError($"{StatusCode.InvalidArgument}, {message}");
            throw new RpcException(new Status(StatusCode.InvalidArgument, message));
        }
        if (string.IsNullOrWhiteSpace(request.Id) || request.Id == Guid.Empty.ToString())
        {
            string message = "Playerr ID cannot be empty";
            logger.LogError($"{StatusCode.InvalidArgument}, {message}");
            throw new RpcException(new Status(StatusCode.InvalidArgument, message));
        }
        return request;
    }
}

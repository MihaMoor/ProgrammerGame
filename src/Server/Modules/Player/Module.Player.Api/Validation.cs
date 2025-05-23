using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.GrpcContracts.V1;

namespace Server.Module.Player.Api;

internal class Validation
{
    /// <summary>
    /// Validates a UUID request, ensuring it is not null and contains a non-empty player ID.
    /// </summary>
    /// <param name="request">The UUID request to validate.</param>
    /// <returns>The validated UUID request.</returns>
    /// <exception cref="RpcException">
    /// Thrown if the request is null or the player ID is missing or empty.
    /// </exception>
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
            string message = "Player ID cannot be empty";
            logger.LogError($"{StatusCode.InvalidArgument}, {message}");
            throw new RpcException(new Status(StatusCode.InvalidArgument, message));
        }
        return request;
    }
}

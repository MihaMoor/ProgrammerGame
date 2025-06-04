using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.GrpcContracts.V1;

namespace Server.Module.Player.Api;

internal class Validation
{
    /// <summary>
    /// ѕровер€ет запрос UUID, обеспечива€ его непустоту и наличие допустимого, непустого идентификатора игрока.
    /// </summary>
    /// <param name="request">«апрос UUID дл€ проверки.</param>
    /// <returns>ѕроверенный запрос UUID.</returns>
    /// <exception cref="RpcException">
    /// ¬озникает, если запрос равен null или если идентификатор игрока равен null, пуст, содержит только пробелы или равен пустому GUID.
    /// </exception>
    internal static UUID Validate(UUID request, ILogger logger)
    {
        if (request == null)
        {
            string message = "Request cannot be null";
            logger.LogError($"{StatusCode.InvalidArgument}, {message}");
            throw new RpcException(new Status(StatusCode.InvalidArgument, message));
        }
        if (string.IsNullOrWhiteSpace(request.PlayerId) || request.PlayerId == Guid.Empty.ToString())
        {
            string message = "Player ID cannot be empty";
            logger.LogError($"{StatusCode.InvalidArgument}, {message}");
            throw new RpcException(new Status(StatusCode.InvalidArgument, message));
        }
        return request;
    }
}

using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Module.Player.GrpcContracts.V1;

namespace Server.Module.Player.Api;

internal class Validation
{
    /// <summary>
    /// ��������� ������ UUID, ����������� ��� ��������� � ������� �����������, ��������� �������������� ������.
    /// </summary>
    /// <param name="request">������ UUID ��� ��������.</param>
    /// <returns>����������� ������ UUID.</returns>
    /// <exception cref="RpcException">
    /// ���������, ���� ������ ����� null ��� ���� ������������� ������ ����� null, ����, �������� ������ ������� ��� ����� ������� GUID.
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

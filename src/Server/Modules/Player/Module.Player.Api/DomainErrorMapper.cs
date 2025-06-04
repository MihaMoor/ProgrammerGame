using Grpc.Core;
using Server.Shared.Errors;

namespace Server.Module.Player.Api;

internal static class DomainErrorMapper
{
    /// <summary>
    /// ������������ �������� <see cref="ErrorCode"/> � ��������������� gRPC-�������� <see cref="StatusCode"/>.
    /// </summary>
    /// <param name="errorCode">��� ������ ���������� ������� ��� �������������.</param>
    /// <returns>������������� gRPC-������ ��� ���������� ���� ������.</returns>
    internal static StatusCode ToStatusCode(this ErrorCode errorCode) =>
        errorCode switch
        {
            ErrorCode.EntityNotFound => StatusCode.NotFound,
            ErrorCode.None => StatusCode.Unknown,
            ErrorCode.IsEmpty => StatusCode.InvalidArgument,
            ErrorCode.NullValue => StatusCode.InvalidArgument,
            _ => StatusCode.Unimplemented,
        };
}

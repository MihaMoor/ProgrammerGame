using Grpc.Core;
using Server.Shared.Errors;

namespace Module.Player.Api;

internal static class DomainErrorMapper
{
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

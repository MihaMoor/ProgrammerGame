using Grpc.Core;
using Server.Shared.Errors;

namespace Server.Module.Player.Api;

internal static class DomainErrorMapper
{
    /// <summary>
        /// Maps an <see cref="ErrorCode"/> value to the corresponding gRPC <see cref="StatusCode"/>.
        /// </summary>
        /// <param name="errorCode">The domain error code to map.</param>
        /// <returns>The equivalent gRPC status code for the specified error code.</returns>
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

using Grpc.Core;
using Server.Shared.Errors;

namespace Server.Module.Player.Api;

internal static class DomainErrorMapper
{
    /// <summary>
    /// Сопоставляет значение <see cref="ErrorCode"/> с соответствующим gRPC-статусом <see cref="StatusCode"/>.
    /// </summary>
    /// <param name="errorCode">Код ошибки предметной области для сопоставления.</param>
    /// <returns>Эквивалентный gRPC-статус для указанного кода ошибки.</returns>
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

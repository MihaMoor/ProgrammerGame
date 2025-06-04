using Microsoft.AspNetCore.Routing;

namespace Shared.EndpointMapper;

public interface IEndpoint
{
    /// <summary>
    /// Ќастраивает и регистрирует конечные точки с использованием предоставленного маршрутизатора (route builder).
    /// </summary>
    /// <param name="app">ћаршрутизатор (route builder), к которому будут добавлены конечные точки.</param>
    void MapEndpoints(IEndpointRouteBuilder app);
}

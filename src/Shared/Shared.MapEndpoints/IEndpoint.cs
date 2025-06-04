using Microsoft.AspNetCore.Routing;

namespace Shared.EndpointMapper;

public interface IEndpoint
{
    /// <summary>
    /// ����������� � ������������ �������� ����� � �������������� ���������������� �������������� (route builder).
    /// </summary>
    /// <param name="app">������������� (route builder), � �������� ����� ��������� �������� �����.</param>
    void MapEndpoints(IEndpointRouteBuilder app);
}

using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Server.Module.Player.GrpcContracts.V1;

namespace Client.Infrastructure;

public static class GrpsServices
{
    public static ServiceCollection ConfigureGrpcServices(
        this ServiceCollection serviceCollection,
        string adress
    )
    {
        serviceCollection.AddSingleton(x => GrpcChannel.ForAddress(adress));

        serviceCollection.ConfigureContractServiceClients().ConfigureGrpcClients(adress);

        return serviceCollection;
    }

    /// <summary>
    /// ������������ gRPC-������ PlayerService ��� scoped-������ � ��������� ��������.
    /// </summary>
    /// <param name="serviceCollection">��������� �������� ��� ���������.</param>
    /// <returns>����������� ��������� ��������.</returns>
    private static ServiceCollection ConfigureContractServiceClients(
        this ServiceCollection serviceCollection
    )
    {
        serviceCollection.AddScoped(
            x => new PlayerService.PlayerServiceClient(
                x.GetRequiredService<GrpcChannel>()
            )
        );

        return serviceCollection;
    }

    /// <summary>
    /// ������������ <c>PlayerGrpcClient</c> ��� scoped-������, ��������� ��������� ����� � ��������� ��������� <c>PlayerServiceClient</c>.
    /// </summary>
    /// <param name="adress">����� gRPC-������� ��� ������������� �������.</param>
    /// <returns>����������� ��������� �������� <c>ServiceCollection</c> � ������������ <c>PlayerGrpcClient</c>.</returns>
    private static ServiceCollection ConfigureGrpcClients(
        this ServiceCollection serviceCollection,
        string adress
    )
    {
        serviceCollection.AddScoped(x =>
        {
            PlayerService.PlayerServiceClient client =
                x.GetRequiredService<PlayerService.PlayerServiceClient>();
            return new Clients.PlayerGrpcClient(adress, client);
        });

        return serviceCollection;
    }
}

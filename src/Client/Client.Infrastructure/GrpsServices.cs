using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Infrastructure;

public static class GrpsServices
{
    public static ServiceCollection ConfigureGrpcServices(this ServiceCollection serviceCollection, string adress)
    {
        serviceCollection.AddSingleton(x => GrpcChannel.ForAddress(adress));

        serviceCollection
            .ConfigureContractServiceClients()
            .ConfigureGrpcClients(adress);

        return serviceCollection;
    }

    private static ServiceCollection ConfigureContractServiceClients(this ServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(x =>
            new Shared.GrpcContracts.PlayerMainStatsService.PlayerMainStatsServiceClient(x.GetRequiredService<GrpcChannel>())
        );

        return serviceCollection;
    }

    private static ServiceCollection ConfigureGrpcClients(this ServiceCollection serviceCollection, string adress)
    {
        serviceCollection.AddScoped(x =>
            {
                var client = x.GetRequiredService<Shared.GrpcContracts.PlayerMainStatsService.PlayerMainStatsServiceClient>();
                return new Clients.PlayerMainStatsGrpcClient(adress, client);
            }
        );

        return serviceCollection;
    }
}

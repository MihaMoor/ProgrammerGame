using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Infrastructure;

public class GrpsServices
{
    public static void ConfigureGrpcServices(ServiceCollection serviceCollection, string adress)
    {
        serviceCollection.AddSingleton(x => GrpcChannel.ForAddress(adress));
        ConfigureContractServiceClients(serviceCollection);
        ConfigureGrpcClients(serviceCollection, adress);
    }

    private static void ConfigureContractServiceClients(ServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(x =>
            new Shared.GrpcContracts.PlayerMainStatsService.PlayerMainStatsServiceClient(x.GetRequiredService<GrpcChannel>())
        );
    }

    private static void ConfigureGrpcClients(ServiceCollection serviceCollection, string adress)
    {
        serviceCollection.AddScoped(x =>
            {
                var client = x.GetRequiredService<Shared.GrpcContracts.PlayerMainStatsService.PlayerMainStatsServiceClient>();
                return new Clients.PlayerMainStatsGrpcClient(adress, client);
            }
        );
    }
}

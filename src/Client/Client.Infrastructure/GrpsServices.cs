using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Server.Module.Player.GrpcContracts.Player.V1;

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

    private static ServiceCollection ConfigureContractServiceClients(
        this ServiceCollection serviceCollection
    )
    {
        serviceCollection.AddScoped(
            x => new PlayerService.PlayerServiceClient(
                x.GetRequiredService<GrpcChannel>()
            )
        );
        serviceCollection.AddScoped(
            x => new CreatePlayerService.CreatePlayerServiceClient(
                x.GetRequiredService<GrpcChannel>()
            )
        );

        return serviceCollection;
    }

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

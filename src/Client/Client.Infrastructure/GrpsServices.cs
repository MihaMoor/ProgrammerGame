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
    /// Registers the PlayerService gRPC client as a scoped service in the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to configure.</param>
    /// <returns>The updated service collection.</returns>
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
    /// Registers a scoped <c>PlayerGrpcClient</c> using the specified address and a resolved <c>PlayerServiceClient</c>.
    /// </summary>
    /// <param name="adress">The gRPC server address used to initialize the client.</param>
    /// <returns>The updated <c>ServiceCollection</c> with the <c>PlayerGrpcClient</c> registration.</returns>
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

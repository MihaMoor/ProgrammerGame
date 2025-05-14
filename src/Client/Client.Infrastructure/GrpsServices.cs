using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;

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
    /// Registers scoped gRPC service clients for player-related operations in the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection to configure.</param>
    /// <returns>The updated service collection with gRPC service clients registered.</returns>
    private static ServiceCollection ConfigureContractServiceClients(
        this ServiceCollection serviceCollection
    )
    {
        serviceCollection.AddScoped(
            x => new Server.Module.Player.GrpcContracts.PlayerService.PlayerServiceClient(
                x.GetRequiredService<GrpcChannel>()
            )
        );
        serviceCollection.AddScoped(
            x => new Server.Module.Player.GrpcContracts.CreatePlayerService.CreatePlayerServiceClient(
                x.GetRequiredService<GrpcChannel>()
            )
        );

        return serviceCollection;
    }

    /// <summary>
    /// Registers the PlayerGrpcClient as a scoped service using the provided address and a resolved PlayerServiceClient.
    /// </summary>
    /// <param name="adress">The gRPC server address used to initialize the client.</param>
    /// <returns>The updated ServiceCollection with the PlayerGrpcClient registration.</returns>
    private static ServiceCollection ConfigureGrpcClients(
        this ServiceCollection serviceCollection,
        string adress
    )
    {
        serviceCollection.AddScoped(x =>
        {
            var client =
                x.GetRequiredService<Server.Module.Player.GrpcContracts.PlayerService.PlayerServiceClient>();
            return new Clients.PlayerGrpcClient(adress, client);
        });

        return serviceCollection;
    }
}

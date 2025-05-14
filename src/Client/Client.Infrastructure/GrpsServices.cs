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
    /// Registers gRPC client services for player-related operations in the service collection.
    /// </summary>
    /// <returns>The updated service collection with player gRPC clients registered.</returns>
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
    /// Registers a scoped <c>PlayerGrpcClient</c> in the service collection, using the specified address and a resolved <c>PlayerServiceClient</c>.
    /// </summary>
    /// <param name="adress">The gRPC server address used to initialize the client.</param>
    /// <returns>The updated <c>ServiceCollection</c> with the client registration.</returns>
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

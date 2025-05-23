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
    /// Registers gRPC client services for player-related operations using the configured <see cref="GrpcChannel"/>.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the gRPC clients to.</param>
    /// <returns>The updated service collection with gRPC client registrations.</returns>
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

    /// <summary>
    /// Registers a scoped <c>PlayerGrpcClient</c> that uses the specified address and a resolved <c>PlayerServiceClient</c>.
    /// </summary>
    /// <param name="adress">The gRPC server address used by the client wrapper.</param>
    /// <returns>The updated <c>ServiceCollection</c> with the client registration.</returns>
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

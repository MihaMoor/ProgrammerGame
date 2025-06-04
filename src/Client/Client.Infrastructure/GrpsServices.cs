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
    /// Регистрирует gRPC-клиент PlayerService как scoped-сервис в коллекции сервисов.
    /// </summary>
    /// <param name="serviceCollection">Коллекция сервисов для настройки.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
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
    /// Регистрирует <c>PlayerGrpcClient</c> как scoped-сервис, используя указанный адрес и созданный экземпляр <c>PlayerServiceClient</c>.
    /// </summary>
    /// <param name="adress">Адрес gRPC-сервера для инициализации клиента.</param>
    /// <returns>Обновленная коллекция сервисов <c>ServiceCollection</c> с регистрацией <c>PlayerGrpcClient</c>.</returns>
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

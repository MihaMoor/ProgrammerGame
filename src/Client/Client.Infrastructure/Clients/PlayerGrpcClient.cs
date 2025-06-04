using Server.Module.Player.GrpcContracts.V1;

namespace Client.Infrastructure.Clients;

public class PlayerGrpcClient(string adress, PlayerService.PlayerServiceClient client)
    : GrpcClient<PlayerService.PlayerServiceClient>(adress, client)
{
    /// <summary>
    /// Получает игрока по фиксированному идентификатору с использованием gRPC, с возможностью обработки результата через указанный обработчик.
    /// </summary>
    /// <param name="handler">Необязательный делегат для обработки полученного <see cref="PlayerDto"/>.</param>
    /// <param name="cancellationToken">Токен отмены для наблюдения за выполнением операции.</param>
    /// <returns>Объект <see cref="PlayerDto"/>, соответствующий фиксированному идентификатору игрока.</returns>
    public async Task<PlayerDto> GetAsync(
        Action<PlayerDto> handler,
        CancellationToken cancellationToken)
    {
        return await RetryPolicy.ExecuteAsync(
            async Task<PlayerDto> () =>
            {
                PlayerDto? dto = null;
                UUID uUID = new() { PlayerId = "01973485-47c7-7cbc-834f-ec060fbb5e75" };
                dto = await Client.GetAsync(uUID, cancellationToken: cancellationToken);

                handler?.Invoke(dto);
                return dto;
            }
        );
    }
}

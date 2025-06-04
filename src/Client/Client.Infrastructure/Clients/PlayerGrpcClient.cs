using Server.Module.Player.GrpcContracts.V1;

namespace Client.Infrastructure.Clients;

public class PlayerGrpcClient(string adress, PlayerService.PlayerServiceClient client)
    : GrpcClient<PlayerService.PlayerServiceClient>(adress, client)
{
    /// <summary>
    /// Retrieves a player by a fixed player ID using gRPC, optionally invoking a handler with the result.
    /// </summary>
    /// <param name="handler">An optional delegate to process the retrieved <see cref="PlayerDto"/>.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>The <see cref="PlayerDto"/> corresponding to the fixed player ID.</returns>
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

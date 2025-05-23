using System.Windows;
using System.Windows.Controls;
using Client.Infrastructure.Clients;
using Server.Module.Player.GrpcContracts.V1;

namespace WpfClient.Widgets;

/// <summary>
/// Interaction logic for .xaml
/// </summary>
public partial class PlayerWidget : Page
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Player _player;
    private readonly PlayerGrpcClient _playerGrpcClient;

    /// <summary>
    /// Initializes a new instance of the PlayerWidget page, sets up data binding, event handlers, and begins fetching player data from the gRPC service.
    /// </summary>
    public PlayerWidget(PlayerGrpcClient grpcClient)
    {
        _player = new();
        _playerGrpcClient = grpcClient;
        _cancellationTokenSource = new();
        InitializeComponent();
        DataContext = _player;
        Unloaded += PageUnloaded;
        ConnectToServer();
    }

    /// <summary>
    /// Initiates an asynchronous request to retrieve player data from the gRPC service.
    /// </summary>
    private void ConnectToServer()
    {
        _playerGrpcClient.Get(HandlePlayerGet, _cancellationTokenSource.Token);
    }

    /// <summary>
    /// Updates the player model's properties on the UI thread using data from the provided player DTO.
    /// </summary>
    /// <param name="response">The player data received from the gRPC service.</param>
    private void HandlePlayerGet(PlayerDto response)
    {
        Dispatcher.Invoke(() =>
        {
            _player.Name = response.Name;
            _player.Health = response.Health;
            _player.Hunger = response.Hunger;
            _player.Mood = response.Mood;
            _player.PocketMoney = response.PocketMoney;
        });
    }

    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }
}

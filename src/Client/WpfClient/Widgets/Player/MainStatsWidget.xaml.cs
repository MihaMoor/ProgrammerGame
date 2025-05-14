using System.Windows;
using System.Windows.Controls;
using Client.Infrastructure.Clients;
using Server.Module.Player.GrpcContracts;

namespace WpfClient.Widgets;

/// <summary>
/// Interaction logic for .xaml
/// </summary>
public partial class MainStatsWidget : Page
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly MainStats _player;
    private readonly PlayerGrpcClient _playerGrpcClient;

    /// <summary>
    /// Initializes the main stats widget, sets up data binding, and starts fetching player statistics from the server.
    /// </summary>
    /// <param name="grpcClient">The gRPC client used to retrieve player statistics.</param>
    public MainStatsWidget(PlayerGrpcClient grpcClient)
    {
        _player = new();
        _playerGrpcClient = grpcClient;
        _cancellationTokenSource = new();
        InitializeComponent();
        DataContext = _player;
        ConnectToServer();
    }

    /// <summary>
    /// Initiates an asynchronous request to fetch player statistics from the server and updates the UI upon receiving data.
    /// </summary>
    private async void ConnectToServer()
    {
        var responce = await _playerGrpcClient.Get(HandlePlayerGet, _cancellationTokenSource.Token);
    }

    /// <summary>
    /// Updates the player's main statistics on the UI thread using data from the provided PlayerDto response.
    /// </summary>
    /// <param name="responce">The player data received from the server.</param>
    private void HandlePlayerGet(PlayerDto responce)
    {
        Dispatcher.Invoke(() =>
        {
            _player.Name = responce.Name;
            _player.Health = responce.Health;
            _player.Hunger = responce.Hunger;
            _player.Mood = responce.Mood;
            _player.PocketMoney = responce.PocketMoney;
        });
    }

    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }
}

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
    /// Initializes the main stats widget, sets up data binding, and starts retrieving player statistics from the server.
    /// </summary>
    /// <param name="grpcClient">The gRPC client used to fetch player data.</param>
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
    /// Initiates a request to retrieve player statistics from the server and registers a callback to handle the response.
    /// </summary>
    private void ConnectToServer()
    {
        PlayerDto response = _playerGrpcClient.Get(HandlePlayerGet, _cancellationTokenSource.Token);
    }

    /// <summary>
    /// Updates the player statistics model with data received from the server on the UI thread.
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

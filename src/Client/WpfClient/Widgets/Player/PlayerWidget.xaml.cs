using System.Windows;
using System.Windows.Controls;
using Client.Infrastructure.Clients;
using Google.Type;
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
    /// Initializes a new instance of the PlayerWidget page, sets up data binding, event handling, and begins retrieving player information from the server.
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
    /// Initiates an asynchronous request to retrieve player data from the server and processes the response using a callback.
    /// </summary>
    private void ConnectToServer()
    {
        _playerGrpcClient.GetAsync(HandlePlayerGet, _cancellationTokenSource.Token);
    }

    /// <summary>
    /// Updates the player model with data received from the server.
    /// </summary>
    /// <param name="response">The player data transfer object containing updated player information.</param>
    private void HandlePlayerGet(PlayerDto response)
    {
        Dispatcher.Invoke(() =>
        {
            _player.Name = response.Name;
            _player.Health = response.Health;
            _player.Hunger = response.Hunger;
            _player.Mood = response.Mood;
            _player.PocketMoney = ConvertGoogleMoney(response.PocketMoney);
        });
    }

    /// <summary>
    /// Handles the page's Unloaded event by canceling any ongoing asynchronous operations.
    /// </summary>
    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }

    /// <summary>
        /// Converts a Google <c>Money</c> object to a <c>double</c> representing the total monetary value.
        /// </summary>
        /// <param name="money">The <c>Money</c> object containing units and nanos.</param>
        /// <returns>The combined monetary value as a <c>double</c>.</returns>
        private double ConvertGoogleMoney(Money money)
        => money.Units + money.Nanos / 1_000_000_000.0;
}

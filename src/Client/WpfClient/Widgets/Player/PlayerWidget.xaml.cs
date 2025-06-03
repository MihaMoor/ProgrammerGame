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

    private void ConnectToServer()
    {
        _playerGrpcClient.Get(HandlePlayerGet, _cancellationTokenSource.Token);
    }

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

    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }

    private double ConvertGoogleMoney(Money money)
        => money.Units + money.Nanos / 1_000_000_000.0;
}

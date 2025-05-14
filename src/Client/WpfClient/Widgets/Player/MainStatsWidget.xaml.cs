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

    public MainStatsWidget(PlayerGrpcClient grpcClient)
    {
        _player = new();
        _playerGrpcClient = grpcClient;
        _cancellationTokenSource = new();
        InitializeComponent();
        DataContext = _player;
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        PlayerDto response = _playerGrpcClient.Get(HandlePlayerGet, _cancellationTokenSource.Token);
    }

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

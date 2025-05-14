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
    private readonly MainStats _mainStats;
    private readonly PlayerGrpcClient _playerGrpcClient;

    public MainStatsWidget(PlayerGrpcClient grpcClient)
    {
        _mainStats = new();
        _playerGrpcClient = grpcClient;
        _cancellationTokenSource = new();
        InitializeComponent();
        DataContext = _mainStats;
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
            _mainStats.Name = responce.Name;
            _mainStats.Health = responce.Health;
            _mainStats.Hunger = responce.Hunger;
            _mainStats.Mood = responce.Mood;
            _mainStats.PocketMoney = responce.PocketMoney;
        });
    }

    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }
}

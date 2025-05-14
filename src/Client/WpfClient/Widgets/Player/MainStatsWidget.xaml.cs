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

    private void HandlePlayerGet(PlayerDto response)
    {
        Dispatcher.Invoke(() =>
        {
            _mainStats.Name = response.Name;
            _mainStats.Health = response.Health;
            _mainStats.Hunger = response.Hunger;
            _mainStats.Mood = response.Mood;
            _mainStats.PocketMoney = response.PocketMoney;
        });
    }

    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }
}

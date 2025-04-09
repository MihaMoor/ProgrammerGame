using Client.Infrastructure.Clients;
using Shared.GrpcContracts;
using System.Windows;
using System.Windows.Controls;

namespace WpfClient.Widgets.MainStats;

/// <summary>
/// Interaction logic for MainStats.xaml
/// </summary>
public partial class MainStats : Page
{
    private readonly PlayerMainStatsGrpcClient _grpcClient;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly PlayerMainStats _playerMainStats;

    public MainStats(PlayerMainStatsGrpcClient grpcClient)
    {
        _playerMainStats = new();
        _grpcClient = grpcClient;
        _cancellationTokenSource = new();
        InitializeComponent();
        DataContext = _playerMainStats;
        ConnectToServer();
    }

    private async void ConnectToServer()
    {
        PlayerMainStatsDto responce = await _grpcClient.GetAsync(HandlePlayerMainStatsGet, _cancellationTokenSource.Token);
    }

    private void HandlePlayerMainStatsGet(PlayerMainStatsDto responce)
    {
        Dispatcher.Invoke(() =>
        {
            _playerMainStats.Health = responce.Health;
            _playerMainStats.Hunger = responce.Hunger;
            _playerMainStats.Money = responce.Money;
            _playerMainStats.Mood = responce.Mood;
        });
    }

    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }
}

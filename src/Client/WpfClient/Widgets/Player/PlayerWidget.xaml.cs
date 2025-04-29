using System.Windows;
using System.Windows.Controls;
using Client.Infrastructure.Clients;
using Shared.GrpcContracts;

namespace WpfClient.Widgets.PlayerWidget;

/// <summary>
/// Interaction logic for .xaml
/// </summary>
public partial class PlayerWidget : Page
{
    private readonly PlayerGrpcClient _grpcClient;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Player _player;

    public PlayerWidget(PlayerGrpcClient grpcClient)
    {
        _player = new();
        _grpcClient = grpcClient;
        _cancellationTokenSource = new();
        InitializeComponent();
        DataContext = _player;
        ConnectToServer();
    }

    private async void ConnectToServer()
    {
        PlayerDto responce = await _grpcClient.GetAsync(
            HandlePlayerGet,
            _cancellationTokenSource.Token
        );
    }

    private void HandlePlayerGet(PlayerDto responce)
    {
        Dispatcher.Invoke(() =>
        {
            _player.Name = responce.Name;
            _player.Health = responce.Health;
            _player.Hunger = responce.Hunger;
            _player.Money = responce.Money;
            _player.Mood = responce.Mood;
            _player.PocketMoney = responce.PocketMoney;
        });
    }

    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }
}

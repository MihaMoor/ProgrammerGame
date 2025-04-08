using Client.Infrastructure.Clients;
using Shared.GrpcContracts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfClient.Widgets.MainStats;

/// <summary>
/// Interaction logic for MainStats.xaml
/// </summary>
public partial class MainStats : Page, INotifyPropertyChanged
{
    private readonly PlayerMainStatsGrpcClient _grpcClient;
    private CancellationTokenSource _cancellationTokenSource;

    private uint _health;
    private uint _hunger;
    private double _money;
    private uint _mood;

    public event PropertyChangedEventHandler? PropertyChanged;

    public uint Health
    {
        get => _health;
        set
        {
            _health = value;
            OnPropertyChanged();
        }
    }
    public uint Hunger
    {
        get => _hunger;
        set
        {
            _hunger = value;
            OnPropertyChanged();
        }
    }
    public double Money
    {
        get => _money;
        set
        {
            _money = Math.Round(value, 2);
            OnPropertyChanged();
        }
    }
    public uint Mood
    {
        get => _mood; set
        {
            _mood = value;
            OnPropertyChanged();
        }
    }

    public MainStats(PlayerMainStatsGrpcClient grpcClient)
    {
        _grpcClient = grpcClient;
        _cancellationTokenSource = new();
        InitializeComponent();
        DataContext = this;
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
            Health = responce.Health;
            Hunger = responce.Hunger;
            Money = responce.Money;
            Mood = responce.Mood;
        });
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void PageUnloaded(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }
}

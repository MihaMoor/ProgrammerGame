using Client.Infrastructure;
using System.ComponentModel;
using System.Windows.Controls;
using Google.Protobuf.WellKnownTypes;

namespace WpfClient.Widgets.MainStats;

/// <summary>
/// Interaction logic for MainStats.xaml
/// </summary>
public partial class MainStats : Page
{
    private GrpcClient<Empty, Shared.GrpcContracts.PlayerMainStatsService.PlayerMainStatsServiceClient> _grpcClient;

    public MainStats(GrpcClient<Empty, Shared.GrpcContracts.PlayerMainStatsService.PlayerMainStatsServiceClient> grpcClient)
    {
        _grpcClient = grpcClient;
        InitializeComponent();
        Init();
    }

    private async void Init()
    {
        var responce = await _grpcClient.Client.GetAsync(new Empty());
        if (responce != null)
        {
            this.DataContext = new PlayerMainStats(health: responce.Health, hunger: responce.Hunger, money: responce.Money, mood: responce.Mood);
        }
        else
        {
            this.DataContext = new PlayerMainStats(health: 100, hunger: 100, money: 99.99, mood: 100);
        }
    }
}

public class PlayerMainStats : INotifyPropertyChanged
{
    private uint _health;
    private uint _hunger;
    private double _money;
    private uint _mood;

    /// <summary>
    /// Здоровье
    /// </summary>
    public uint Health
    {
        get => _health;
        set
        {
            _health =
            value < 0
            ? 0
            : value > 100
                ? 100
                : value;
            OnPropertyChanged(nameof(Health));
        }
    }

    /// <summary>
    /// Голод
    /// </summary>
    public uint Hunger
    {
        get => _hunger;
        set
        {
            _hunger =
            value < 0
            ? 0
            : value > 100
                ? 100
                : value;
            OnPropertyChanged(nameof(Hunger));
        }
    }

    /// <summary>
    /// Деньги
    /// </summary>
    public double Money
    {
        get => _money;
        set
        {
            SetMoney(value);
            OnPropertyChanged(nameof(Money));
        }
    }

    /// <summary>
    /// Настроение
    /// </summary>
    public uint Mood
    {
        get => _mood;
        set
        {
            _mood =
            value < 0
            ? 0
            : value > 100
                ? 100
                : value;
            OnPropertyChanged(nameof(Mood));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;


    public PlayerMainStats(uint health, uint hunger, double money, uint mood)
    {
        Health = health;
        Hunger = hunger;
        Mood = mood;
        _money = money;
    }

    protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void SetMoney(double value)
    {
        if (value != double.MinValue &&
            value != double.MaxValue &&
            value != double.NegativeInfinity &&
            value != double.PositiveInfinity &&
            !double.IsNaN(value))
            _money += value;
    }
}

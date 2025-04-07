using System.ComponentModel;

namespace WpfClient.Widgets.MainStats;

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

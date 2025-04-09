using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfClient.Widgets.MainStats;

internal class PlayerMainStats : INotifyPropertyChanged
{
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

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

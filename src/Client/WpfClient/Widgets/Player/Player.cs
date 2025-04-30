using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfClient.Widgets.PlayerWidget;

internal class Player : INotifyPropertyChanged
{
    private uint _health;
    private uint _hunger;
    private double _money;
    private uint _mood;
    private double _pocketMoney;
    private string _name;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public double PocketMoney
    {
        get => _pocketMoney;
        set
        {
            _pocketMoney = value;
            OnPropertyChanged();
        }
    }

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
        get => _mood;
        set
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

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfClient.Widgets;

internal sealed class Player : INotifyPropertyChanged
{
    private uint _health;
    private uint _hunger;
    private uint _mood;
    private double _pocketMoney;
    private string _name = "Unknown";

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

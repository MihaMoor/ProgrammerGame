using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfClient.Widgets;

internal sealed class Player : INotifyPropertyChanged
{
    private int _health;
    private int _hunger;
    private int _mood;
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

    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            OnPropertyChanged();
        }
    }
    public int Hunger
    {
        get => _hunger;
        set
        {
            _hunger = value;
            OnPropertyChanged();
        }
    }
    public int Mood
    {
        get => _mood;
        set
        {
            _mood = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Raises the PropertyChanged event to notify listeners that a property value has changed.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed. Automatically set to the caller's name if not specified.</param>
    internal void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

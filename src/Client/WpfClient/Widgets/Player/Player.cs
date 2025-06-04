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

    /// <summary>
    /// Имя
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Карманные деньги
    /// </summary>
    public double PocketMoney
    {
        get => _pocketMoney;
        set
        {
            _pocketMoney = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Здоровье
    /// </summary>
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Голод
    /// </summary>
    public int Hunger
    {
        get => _hunger;
        set
        {
            _hunger = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Настроение
    /// </summary>
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
    /// Вызывает событие PropertyChanged для уведомления подписчиков об изменении значения свойства.
    /// </summary>
    /// <param name="propertyName">Имя изменившегося свойства. Автоматически устанавливается в имя вызывающего свойства, если не указано явно.</param>
    internal void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

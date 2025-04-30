namespace Server.Player.Domain;

public class Player
{
    private uint _health;
    private uint _hunger;
    private uint _mood;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Здоровье
    /// </summary>
    public uint Health
    {
        get => _health;
        set =>
            _health = value switch
            {
                > 100 => 100,
                <= 0 => 0,
                _ => value,
            };
    }

    /// <summary>
    /// Голод
    /// </summary>
    public uint Hunger
    {
        get => _hunger;
        set =>
            _hunger = value switch
            {
                > 100 => 100,
                <= 0 => 0,
                _ => value,
            };
    }

    /// <summary>
    /// Настроение
    /// </summary>
    public uint Mood
    {
        get => _mood;
        set =>
            _mood = value switch
            {
                > 100 => 100,
                <= 0 => 0,
                _ => value,
            };
    }

    /// <summary>
    /// Карманные деньги
    /// </summary>
    public double PocketMoney { get; set; }
}

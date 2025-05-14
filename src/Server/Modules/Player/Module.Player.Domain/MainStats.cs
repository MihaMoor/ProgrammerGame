namespace Server.Module.Player.Domain;

public class MainStats
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid MainStatsId { get; init; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Здоровье
    /// </summary>
    public uint Health { get; private set; }

    /// <summary>
    /// Голод
    /// </summary>
    public uint Hunger { get; private set; }

    /// <summary>
    /// Настроение
    /// </summary>
    public uint Mood { get; private set; }

    /// <summary>
    /// Карманные деньги
    /// </summary>
    public double PocketMoney { get; private set; }

    private MainStats()
    {
        Name = string.Empty;
    }

    public static MainStats CreatePlayer(string name)
    {
        if (name == string.Empty)
            throw new ArgumentNullException("Имя игрока не может быть пустым");

        return new MainStats
        {
            Name = name,
            Health = 100,
            Hunger = 100,
            Mood = 100,
            PocketMoney = 99.99,
        };
    }
}

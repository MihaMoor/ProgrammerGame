namespace Module.Player.Domain;

public class Player
{
    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Здоровье
    /// </summary>
    public uint Health { get; set; }

    /// <summary>
    /// Голод
    /// </summary>
    public uint Hunger { get; set; }

    /// <summary>
    /// Настроение
    /// </summary>
    public uint Mood { get; set; }

    /// <summary>
    /// Карманные деньги
    /// </summary>
    public double PocketMoney { get; set; }

    private Player() { }

    public static Player CreatePlayer(string name)
    {
        return new Player { Name = name };
    }
}

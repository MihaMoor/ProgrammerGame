namespace Server.WorkPlace.Domain;

public enum WirelessFrequency
{
    Ggh24,
    Ggh50,
}

public class WirelessRouter
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Поддерживаемые частоты Wi-Fi
    /// </summary>
    public List<WirelessFrequency> Frequencies { get; set; }

    /// <summary>
    /// Поддерживаемые стандарты Wi-Fi для каждой частоты
    /// </summary>
    public Dictionary<WirelessFrequency, List<InterfaceType>> WifiStandarts { get; set; }

    /// <summary>
    /// Интерфейсы подключения к роутеру
    /// </summary>
    public List<InterfaceType> Ports { get; set; }

    /// <summary>
    /// Максимальная скорость на каждой частоте
    /// </summary>
    public Dictionary<WirelessFrequency, uint> WifiMaxSpeed { get; set; }

    /// <summary>
    /// Максимальная скорость при подключении через кабель
    /// </summary>
    public uint LanMaxSpeed { get; set; }
}

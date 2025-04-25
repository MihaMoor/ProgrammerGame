namespace Server.Player.Domain;

public class Headphone
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }
    /// <summary>
    /// Максимальная мощность, dB
    /// </summary>
    public float MaxPower { get; set; }
    /// <summary>
    /// Тип подключения
    /// </summary>
    public InterfaceType InterfaceType { get; set; }
}
namespace Server.Player.Domain;

public class Monitor
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }
    /// <summary>
    /// Максимальное разрешение
    /// </summary>
    public (uint,uint) MaxResolution { get; set; }
    /// <summary>
    /// Интерфейс подключения
    /// </summary>
    public List<InterfaceType> InterfaceTypes { get; set; }
}

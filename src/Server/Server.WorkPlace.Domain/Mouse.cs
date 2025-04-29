namespace Server.WorkPlace.Domain;

public class Mouse
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }
    /// <summary>
    /// DPI
    /// </summary>
    public uint Dpi { get; set; }
    /// <summary>
    /// Интерфейс подключения
    /// </summary>
    public InterfaceType InterfaceType { get; set; }
}
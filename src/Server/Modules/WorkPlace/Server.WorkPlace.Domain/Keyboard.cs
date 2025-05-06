namespace Server.WorkPlace.Domain;

public enum KeyboardType
{
    Membran,
    Mechanics,
}

public class Keyboard
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Количество клавиш
    /// </summary>
    public uint KeysCount { get; set; }

    /// <summary>
    /// Тип клавиатуры
    /// </summary>
    public KeyboardType KeyboardType { get; set; }

    /// <summary>
    /// Интерфейс подключения
    /// </summary>
    public InterfaceType InterfaceType { get; set; }
}

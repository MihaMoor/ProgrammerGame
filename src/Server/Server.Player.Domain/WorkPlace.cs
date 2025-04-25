namespace Server.Player.Domain;

public class WorkPlace
{
    /// <summary>
    /// ПК
    /// </summary>
    public List<Computer> Computers { get; set; }
    /// <summary>
    /// Аквтивный (рабочий) ПК
    /// </summary>
    public Computer ActiveComputer { get; set; }
    /// <summary>
    /// Мониторы
    /// </summary>
    public List<Monitor> Monitors { get; private set; }
    /// <summary>
    /// Мышь
    /// </summary>
    public Mouse Mouse { get; set; }
    /// <summary>
    /// Клавиатура
    /// </summary>
    public Keyboard Keyboard { get; set; }
    /// <summary>
    /// Роутер
    /// </summary>
    public WirelessRouter Router { get; set; }
    /// <summary>
    /// Источник бесперебойного питания
    /// </summary>
    public UninterruptiblePowerSupply Ups { get; set; }
    /// <summary>
    /// Наушники
    /// </summary>
    public Headphone Headphones { get; set; }
}

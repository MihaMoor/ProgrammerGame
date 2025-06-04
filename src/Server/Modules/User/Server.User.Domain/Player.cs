namespace Server.User.Domain;

public class Player
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Количество рабочих мест
    /// </summary>
    public uint WorkPlaceCount { get; set; }

    /// <summary>
    /// Наличие работоспособного компьютера
    /// </summary>
    public bool HaveWorkableComputer { get; set; }
}

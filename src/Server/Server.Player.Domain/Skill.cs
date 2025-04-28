namespace Server.Player.Domain;

public class Skill
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Уровень
    /// </summary>
    public uint Level { get; set; }
    /// <summary>
    /// Теукщий прогресс, %
    /// </summary>
    public float CurrentLevelProgress { get; set; }
}
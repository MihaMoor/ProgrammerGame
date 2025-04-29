namespace Server.Education.Domain;

public class Education
{
    public Guid Id { get; set; }
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Длительность обучения
    /// </summary>
    public uint TrainingDuration { get; set; }
}
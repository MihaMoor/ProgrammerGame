namespace Server.Job.Domain;

public class Job
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Зарплата в месяц
    /// </summary>
    public double MonthlySalary { get; set; }
    /// <summary>
    /// Стаж работы
    /// </summary>
    public float WorkExperience { get; set; }
}
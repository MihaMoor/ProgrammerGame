namespace Server.Player.Domain;

public class Player
{
    public string Name { get; set; }
    public MainStats MainStats { get; set; }
    public WorkPlace ActiveWorkPlace { get; set; }
    public List<WorkPlace> WorkPlace { get; set; }
    public Job Job { get; set; }
    public List<Education> Educations { get; set; }
    public List<Skill> Skills { get; set; }
    public Finance Finance { get; set; }
}

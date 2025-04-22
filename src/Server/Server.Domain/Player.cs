namespace Server.Domain;

public class Player
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public MainStats MainStats { get; set; }
    public WorkPlace WorkPlace { get; set; }
    public Job Job { get; set; }
    public Education Education { get; set; }
    public Skills Skills { get; set; }
    public Bank Bank { get; set; }
}

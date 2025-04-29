namespace Server.User.Domain;

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public uint WorkPlaceCount { get; set; }
    public bool HaveWorkableComputer { get; set; }
}

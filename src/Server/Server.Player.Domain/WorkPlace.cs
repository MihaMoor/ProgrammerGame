namespace Server.Player.Domain;

public class WorkPlace
{
    public Computer Computer { get; set; }
    public Monitor Monitor { get; set; }
    public Mouse Mouse { get; set; }
    public Keyboard Keyboard { get; set; }
    public WirelessRouter Router { get; set; }
    public UninterruptiblePowerSupply Ups { get; set; }
    public Headphones Headphones { get; set; }
}

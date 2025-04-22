namespace Server.Domain;

public class MainStats
{
    private uint _health;
    private uint _hunger;
    private double _money;
    private uint _mood;

    public uint Health
    {
        get => _health;
        set =>
            _health =
                value switch
                {
                    > 100 => 100,
                    <= 0 => 0,
                    _ => value
                };
    }

    public uint Hunger
    {
        get => _hunger;
        set =>
            _hunger =
                value switch
                {
                    > 100 => 100,
                    <= 0 => 0,
                    _ => value
                };
    }

    public double Money
    {
        get => _money;
        set => _money += value;
    }

    public uint Mood
    {
        get => _mood;
        set =>
            _mood =
            value switch
            {
                > 100 => 100,
                <= 0 => 0,
                _ => value
            };
    }

    public MainStats(uint health, uint hunger, double money, uint mood)
    {
        Health = health;
        Hunger = hunger;
        Money = money;
        Mood = mood;
    }
}

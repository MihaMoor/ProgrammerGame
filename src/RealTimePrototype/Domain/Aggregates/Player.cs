namespace RealTimePrototype.Domain.Aggregates;

public class Player
{
    private float _satiety;

    public required int Id { get; init; }

    // Голод
    public required float Satiety
    {
        get => _satiety;
        set
        {
            _satiety =
                value switch
                {
                    > 100 => 100,
                    < 0 => 0,
                    _ => value
                };
        }
    }
}

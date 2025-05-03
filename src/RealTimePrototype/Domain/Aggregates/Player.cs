namespace RealTimePrototype.Domain.Aggregates;

public class Player
{
    private float _satiety;

    private float _mood;

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

    // Настроение
    public required float Mood
    {
        get => _mood;
        set
        {
            _mood =
                value switch
                {
                    > 100 => 100,
                    < 0 => 0,
                    _ => value
                };
        }
    }
}

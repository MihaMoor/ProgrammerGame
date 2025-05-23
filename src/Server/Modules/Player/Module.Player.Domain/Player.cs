using Server.Shared.Errors;

namespace Server.Module.Player.Domain;

public sealed class Player
{
    public event Action<Player>? StatsChanged;

    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid PlayerId { get; init; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Здоровье
    /// </summary>
    public uint Health { get; private set; }

    /// <summary>
    /// Голод
    /// </summary>
    public uint Hunger { get; private set; }

    /// <summary>
    /// Настроение
    /// </summary>
    public uint Mood { get; private set; }

    /// <summary>
    /// Карманные деньги
    /// </summary>
    public decimal PocketMoney { get; private set; }

    /// <summary>
    /// Статус, жив ли игрок
    /// </summary>
    public bool IsAlive { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Player class with default values.
    /// </summary>
    private Player()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Создание основных характеристик
    /// </summary>
    /// <param name="name">Имя персонажа</param>
    /// <returns>
    /// <see cref="Result"/> <see cref="PlayerError.NameIsEmpty()"/>
    /// <summary>
    /// Creates a new player with the specified name and default stats.
    /// </summary>
    /// <param name="name">The name of the player to create.</param>
    /// <returns>
    /// A <c>Result&lt;Player&gt;</c> containing the new player if successful, or a failure result if the name is null, empty, or whitespace.
    /// </returns>
    public static Result<Player> CreatePlayer(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Player>(PlayerError.NameIsEmpty());

        Player mainStats = new()
        {
            PlayerId = Guid.NewGuid(),
            Name = name,
            Health = 100,
            Hunger = 100,
            Mood = 100,
            PocketMoney = 99.99m,
            IsAlive = true
        };

        return Result.Success(mainStats);
    }

    /// <summary>
    /// Изменяет значение здоровья с ограничением от 0 до 100
    /// <summary>
    /// Increases the player's health by the specified amount, clamping the result between 0 and 100. Triggers the StatsChanged event if the health value changes. Negative deltas are ignored.
    /// </summary>
    /// <param name="delta">The amount to increase health by; must be non-negative.</param>
    public void ChangeHealth(int delta)
    {
        if(delta < 0)
            return;

        uint prevValue = Health;
        int newValue = (int)Health + delta;
        Health = (uint)Math.Clamp(newValue, 0, 100);

        if (Health != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет значение голода с ограничением от 0 до 100
    /// <summary>
    /// Adjusts the player's hunger level by the specified amount, clamping the result between 0 and 100. Triggers the StatsChanged event if the value changes.
    /// </summary>
    /// <param name="delta">The amount to change the hunger level by (can be positive or negative).</param>
    public void ChangeHunger(int delta)
    {
        uint prevValue = Hunger;
        int newValue = (int)Hunger + delta;
        Hunger = (uint)Math.Clamp(newValue, 0, 100);

        if (Hunger != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет значение настроения с ограничением от 0 до 100
    /// <summary>
    /// Adjusts the player's mood by the specified delta, clamping the result between 0 and 100. Triggers the StatsChanged event if the mood value changes.
    /// </summary>
    /// <param name="delta">The amount to change the mood by (can be positive or negative).</param>
    public void ChangeMood(int delta)
    {
        uint prevValue = Mood;
        int newValue = (int)Mood + delta;
        Mood = (uint)Math.Clamp(newValue, 0, 100);

        if (Mood != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет количество карманных денег
    /// <summary>
    /// Adjusts the player's pocket money by the specified amount, ensuring it does not fall below zero. Triggers the StatsChanged event if the value changes.
    /// </summary>
    /// <param name="delta">The amount to add to the player's pocket money. Can be negative.</param>
    public void ChangePocketMoney(double delta)
    {
        decimal prevValue = PocketMoney;
        PocketMoney = Math.Max(0, PocketMoney + (decimal)delta);

        if (PocketMoney != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет статус жив ли игрок
    /// </summary>
    /// <summary>
    /// Updates the player's alive status and triggers the StatsChanged event if the status changes.
    /// </summary>
    /// <param name="isAlive">The new alive status to set for the player.</param>
    public void ChangeIsAlive(bool isAlive)
    {
        bool prevValue = IsAlive;
        IsAlive = isAlive;

        if(IsAlive != isAlive)
            StatsChanged?.Invoke(this);
    }
}

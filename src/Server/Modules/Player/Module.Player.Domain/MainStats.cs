using Server.Module.Player.Application;
using Server.Shared.Results;

namespace Server.Module.Player.Domain;

public class MainStats
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid MainStatsId { get; init; }

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
    public double PocketMoney { get; private set; }

    /// <summary>
    /// Initializes a new instance of the MainStats class with default values.
    /// </summary>
    private MainStats()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Creates a new <see cref="MainStats"/> instance for a player with the specified name and default stat values.
    /// </summary>
    /// <param name="name">The player's name. Must not be null, empty, or whitespace.</param>
    /// <returns>
    /// A <see cref="Result{MainStats}"/> containing the new player stats if the name is valid; otherwise, a failure result.
    /// </returns>
    public static Result<MainStats> CreatePlayer(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<MainStats>(MainStatsError.NameIsEmpty());

        return new MainStats
        {
            MainStatsId = Guid.NewGuid(),
            Name = name,
            Health = 100,
            Hunger = 100,
            Mood = 100,
            PocketMoney = 99.99,
        };
    }

    /// <summary>
    /// Изменяет значение здоровья с ограничением от 0 до 100
    /// <summary>
    /// Adjusts the player's health by the specified amount, clamping the result between 0 and 100.
    /// </summary>
    /// <param name="delta">The amount to change the health by. Can be positive or negative.</param>
    public void ChangeHealth(int delta)
    {
        int newValue = (int)Health + delta;
        Health = (uint)Math.Clamp(newValue, 0, 100);
    }

    /// <summary>
    /// Изменяет значение голода с ограничением от 0 до 100
    /// <summary>
    /// Adjusts the player's hunger level by the specified amount, clamping the result between 0 and 100.
    /// </summary>
    /// <param name="delta">The amount to change the hunger level by. Positive values increase hunger, negative values decrease it.</param>
    public void ChangeHunger(int delta)
    {
        int newValue = (int)Hunger + delta;
        Hunger = (uint)Math.Clamp(newValue, 0, 100);
    }

    /// <summary>
    /// Изменяет значение настроения с ограничением от 0 до 100
    /// <summary>
    /// Adjusts the player's mood by the specified delta, clamping the result between 0 and 100.
    /// </summary>
    /// <param name="delta">The amount to change the mood by; can be positive or negative.</param>
    public void ChangeMood(int delta)
    {
        int newValue = (int)Mood + delta;
        Mood = (uint)Math.Clamp(newValue, 0, 100);
    }

    /// <summary>
    /// Изменяет количество карманных денег
    /// <summary>
    /// Adjusts the player's pocket money by the specified amount, ensuring it does not fall below zero.
    /// </summary>
    /// <param name="delta">The amount to add to or subtract from the current pocket money.</param>
    public void ChangePocketMoney(double delta)
    {
        PocketMoney = Math.Max(0, PocketMoney + delta);
    }
}

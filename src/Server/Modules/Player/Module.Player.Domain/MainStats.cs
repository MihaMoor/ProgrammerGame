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

    private MainStats()
    {
        Name = string.Empty;
    }

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
    /// </summary>
    public void ChangeHealth(int delta)
    {
        int newValue = (int)Health + delta;
        Health = (uint)Math.Clamp(newValue, 0, 100);
    }

    /// <summary>
    /// Изменяет значение голода с ограничением от 0 до 100
    /// </summary>
    public void ChangeHunger(int delta)
    {
        int newValue = (int)Hunger + delta;
        Hunger = (uint)Math.Clamp(newValue, 0, 100);
    }

    /// <summary>
    /// Изменяет значение настроения с ограничением от 0 до 100
    /// </summary>
    public void ChangeMood(int delta)
    {
        int newValue = (int)Mood + delta;
        Mood = (uint)Math.Clamp(newValue, 0, 100);
    }

    /// <summary>
    /// Изменяет количество карманных денег
    /// </summary>
    public void ChangePocketMoney(double delta)
    {
        PocketMoney = Math.Max(0, PocketMoney + delta);
    }
}

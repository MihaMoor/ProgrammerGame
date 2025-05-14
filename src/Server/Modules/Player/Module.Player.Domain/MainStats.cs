using Server.Shared.Results;

namespace Server.Module.Player.Domain;

public sealed class MainStats
{
    public event Action<MainStats>? StatsChanged;

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

        MainStats mainStats = new()
        {
            MainStatsId = Guid.NewGuid(),
            Name = name,
            Health = 100,
            Hunger = 100,
            Mood = 100,
            PocketMoney = 99.99,
        };

        mainStats.StatsChanged?.Invoke(mainStats);

        return mainStats;
    }

    /// <summary>
    /// Изменяет значение здоровья с ограничением от 0 до 100
    /// </summary>
    public void ChangeHealth(int delta)
    {
        uint prevValue = Health;
        int newValue = (int)Health + delta;
        Health = (uint)Math.Clamp(newValue, 0, 100);

        if (Health != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет значение голода с ограничением от 0 до 100
    /// </summary>
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
    /// </summary>
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
    /// </summary>
    public void ChangePocketMoney(double delta)
    {
        double prevValue = PocketMoney;
        PocketMoney = Math.Max(0, PocketMoney + delta);

        if (PocketMoney != prevValue)
            StatsChanged?.Invoke(this);
    }
}

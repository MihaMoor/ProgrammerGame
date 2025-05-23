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
    /// </summary>
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
        decimal prevValue = PocketMoney;
        PocketMoney = Math.Max(0, PocketMoney + (decimal)delta);

        if (PocketMoney != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет статус жив ли игрок
    /// </summary>
    /// <param name="isAlive"></param>
    public void ChangeIsAlive(bool isAlive)
    {
        bool prevValue = IsAlive;
        IsAlive = isAlive;

        if(IsAlive != isAlive)
            StatsChanged?.Invoke(this);
    }
}

using Server.Shared.Errors;

namespace Server.Module.Player.Domain;

public sealed class Player
{
    public event Action<Player>? StatsChanged;

    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid PlayerId { get; private set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Здоровье
    /// </summary>
    public int Health { get; private set; }

    /// <summary>
    /// Голод
    /// </summary>
    public int Hunger { get; private set; }

    /// <summary>
    /// Настроение
    /// </summary>
    public int Mood { get; private set; }

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
        Name = "Unknown";
    }

    public static Result<Player> CreatePlayer(
        Guid playerId,
        string name,
        int health,
        int hunger,
        int mood,
        decimal pocketMoney,
        bool isAlive)
    {
        List<Result> validations = [];

        if (isAlive)
        {
            validations.AddRange(
                [
                    ValidateName(name),
                    ValidateHealth(health),
                    ValidateHunger(hunger),
                    ValidateMood(mood)
                ]
            );
        }
        else
        {
            validations.Add(ValidateName(name));
        }

        if (validations.Count != 0)
        {
            return Result.Failure<Player>(
                PlayerError.InitializationFailed(
                    validations
                    .Where(x => x.IsFailure)
                    .Select(x => x.Error)
                    .ToList()
                    )
                );
        }

        Player player = new()
        {
            PlayerId = playerId,
            Name = name,
            Health = health,
            Hunger = hunger,
            Mood = mood,
            PocketMoney = pocketMoney,
            IsAlive = isAlive
        };

        return Result.Success(player);
    }

    /// <summary>
    /// Создание основных характеристик
    /// </summary>
    /// <param name="name">Имя персонажа</param>
    /// <returns>
    /// <see cref="Error"/> <see cref="PlayerError.NameIsEmpty()"/>
    /// </returns>
    public static Result<Player> CreatePlayer(string name)
    {
        Result validationNameResult = ValidateName(name);
        if (validationNameResult.IsFailure)
        {
            return Result.Failure<Player>(validationNameResult.Error);
        }

        Player mainStats = new()
        {
            PlayerId = Guid.CreateVersion7(),
            Name = name,
            Health = 100,
            Mood = 100,
            Hunger = 0,
            PocketMoney = 99.99m,
            IsAlive = true,
        };

        return Result.Success(mainStats);
    }

    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PlayerError.NameIsEmpty());
        return Result.Success(name);
    }

    private static Result ValidateHealth(int health)
    {
        if (health < 0 || health > 100)
        {
            return Result.Failure(PlayerError.HealthOutOfRange());
        }
        return Result.Success(health);
    }

    private static Result ValidateMood(int mood)
    {
        if (mood < 0 || mood > 100)
        {
            return Result.Failure(PlayerError.MoodOutOfRange());
        }
        return Result.Success(mood);
    }

    private static Result ValidateHunger(int hunger)
    {
        if (hunger < 0 || hunger > 100)
        {
            return Result.Failure(PlayerError.HungerOutOfRange());
        }
        return Result.Success(hunger);
    }

    /// <summary>
    /// Изменяет значение здоровья с ограничением от 0 до 100
    /// </summary>
    public void ChangeHealth(int delta)
    {
        int prevValue = Health;
        int newValue = Health + delta;
        Health = Math.Clamp(newValue, 0, 100);

        if (Health != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет значение голода с ограничением от 0 до 100
    /// </summary>
    public void ChangeHunger(int delta)
    {
        int prevValue = Hunger;
        int newValue = Hunger + delta;
        Hunger = Math.Clamp(newValue, 0, 100);

        if (Hunger != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет значение настроения с ограничением от 0 до 100
    /// </summary>
    public void ChangeMood(int delta)
    {
        int prevValue = Mood;
        int newValue = Mood + delta;
        Mood = Math.Clamp(newValue, 0, 100);

        if (Mood != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет количество карманных денег
    /// </summary>
    public void ChangePocketMoney(decimal delta)
    {
        decimal prevValue = PocketMoney;
        PocketMoney = Math.Max(0, PocketMoney + delta);

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

        if (IsAlive != prevValue)
            StatsChanged?.Invoke(this);
    }
}

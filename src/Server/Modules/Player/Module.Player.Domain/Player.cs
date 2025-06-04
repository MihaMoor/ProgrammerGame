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

    /// <summary>
    /// Initializes a new instance of the Player class with the name set to "Unknown".
    /// </summary>
    private Player()
    {
        Name = "Unknown";
    }

    /// <summary>
    /// Creates a new <see cref="Player"/> instance with the specified attributes, performing validation on all fields.
    /// </summary>
    /// <param name="playerId">The unique identifier for the player.</param>
    /// <param name="name">The player's name.</param>
    /// <param name="health">The player's health value (0-100 if alive).</param>
    /// <param name="hunger">The player's hunger value (0-100 if alive).</param>
    /// <param name="mood">The player's mood value (0-100 if alive).</param>
    /// <param name="pocketMoney">The player's pocket money amount.</param>
    /// <param name="isAlive">Indicates whether the player is alive.</param>
    /// <returns>
    /// A <see cref="Result{Player}"/> containing the created player if all validations pass; otherwise, a failure result with aggregated validation errors.
    /// </returns>
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

        if (validations.Where(x => x.IsFailure).Any())
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
    /// <summary>
    /// Creates a new player with the specified name and default stats.
    /// </summary>
    /// <param name="name">The name of the player to create.</param>
    /// <returns>A <see cref="Result{Player}"/> containing the new player if successful, or an error if the name is invalid.</returns>
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

    /// <summary>
    /// Validates that the provided name is not null, empty, or whitespace.
    /// </summary>
    /// <param name="name">The name to validate.</param>
    /// <returns>A successful result if the name is valid; otherwise, a failure result with a name error.</returns>
    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PlayerError.NameIsEmpty());
        return Result.Success(name);
    }

    /// <summary>
    /// Validates that the health value is within the range 0 to 100.
    /// </summary>
    /// <param name="health">The health value to validate.</param>
    /// <returns>A successful result if the value is valid; otherwise, a failure result with a health out-of-range error.</returns>
    private static Result ValidateHealth(int health)
    {
        if (health < 0 || health > 100)
        {
            return Result.Failure(PlayerError.HealthOutOfRange());
        }
        return Result.Success(health);
    }

    /// <summary>
    /// Validates that the mood value is within the range 0 to 100.
    /// </summary>
    /// <param name="mood">The mood value to validate.</param>
    /// <returns>A successful result if the mood is valid; otherwise, a failure result with a mood out-of-range error.</returns>
    private static Result ValidateMood(int mood)
    {
        if (mood < 0 || mood > 100)
        {
            return Result.Failure(PlayerError.MoodOutOfRange());
        }
        return Result.Success(mood);
    }

    /// <summary>
    /// Validates that the hunger value is within the range 0 to 100.
    /// </summary>
    /// <param name="hunger">The hunger value to validate.</param>
    /// <returns>A successful result if the value is valid; otherwise, a failure result with a hunger out-of-range error.</returns>
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
    /// <summary>
    /// Adjusts the player's health by the specified amount, clamping the result between 0 and 100. Triggers the StatsChanged event if the health value changes.
    /// </summary>
    /// <param name="delta">The amount to change the health by. Positive values increase health; negative values decrease it.</param>
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
    /// <summary>
    /// Adjusts the player's hunger level by the specified amount, clamping the result between 0 and 100.
    /// Triggers the StatsChanged event if the hunger value changes.
    /// </summary>
    /// <param name="delta">The amount to change the hunger level by. Positive values increase hunger, negative values decrease it.</param>
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
    /// <summary>
    /// Adjusts the player's mood by the specified amount, clamping the result between 0 and 100. Triggers the StatsChanged event if the mood value changes.
    /// </summary>
    /// <param name="delta">The amount to change the mood by.</param>
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
    /// <summary>
    /// Adjusts the player's pocket money by the specified amount, ensuring it does not fall below zero. Triggers the StatsChanged event if the value changes.
    /// </summary>
    /// <param name="delta">The amount to add to the player's pocket money. Can be negative.</param>
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
    /// <summary>
    /// Updates the player's alive status and triggers the StatsChanged event if the value changes.
    /// </summary>
    /// <param name="isAlive">The new alive status to set for the player.</param>
    public void ChangeIsAlive(bool isAlive)
    {
        bool prevValue = IsAlive;
        IsAlive = isAlive;

        if (IsAlive != prevValue)
            StatsChanged?.Invoke(this);
    }
}

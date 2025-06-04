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
    /// Инициализирует новый экземпляр класса <see cref="Player"/> с именем, установленным на "Unknown".
    /// </summary>
    private Player()
    {
        Name = "Unknown";
    }

    /// <summary>
    /// Создает новый экземпляр <see cref="Player"/> с указанными атрибутами, выполняя проверку всех свойств.
    /// </summary>
    /// <param name="playerId">Уникальный идентификатор игрока.</param>
    /// <param name="name">Имя игрока. Не должно быть пустым или содержать только пробелы.</param>
    /// <param name="health">Уровень здоровья игрока (от 0 до 100, если жив).</param>
    /// <param name="hunger">Уровень голода игрока (от 0 до 100, если жив).</param>
    /// <param name="mood">Уровень настроения игрока (от 0 до 100, если жив).</param>
    /// <param name="pocketMoney">Количество карманных денег игрока.</param>
    /// <param name="isAlive">Указывает, жив ли игрок. Если true, проверяются здоровье, голод и настроение.</param>
    /// <returns>
    /// <see cref="Result{Player}"/>, содержащий созданного игрока в случае успеха, или результат сбоя с агрегированными ошибками проверки.
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
    /// Создает нового игрока с указанным именем и стандартными характеристиками.
    /// </summary>
    /// <param name="name">Имя создаваемого игрока.</param>
    /// <returns>
    /// <see cref="Result{Player}"/>, содержащий нового игрока, если имя допустимо; в противном случае, результат сбоя с ошибкой проверки.
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

    /// <summary>
    /// Проверяет, что предоставленное имя игрока не является пустым, не содержит только пробелы или не равно null.
    /// </summary>
    /// <param name="name">Имя игрока для проверки.</param>
    /// <returns>Успешный результат, если имя допустимо; в противном случае, результат сбоя с конкретной ошибкой.</returns>
    private static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PlayerError.NameIsEmpty());
        return Result.Success(name);
    }

    /// <summary>
    /// Проверяет, что значение здоровья находится в диапазоне от 0 до 100 включительно.
    /// </summary>
    /// <param name="health">Значение здоровья для проверки.</param>
    /// <returns>Успешный результат, если значение допустимо; в противном случае, результат сбоя с ошибкой выхода за пределы диапазона.</returns>
    private static Result ValidateHealth(int health)
    {
        if (health < 0 || health > 100)
        {
            return Result.Failure(PlayerError.HealthOutOfRange());
        }
        return Result.Success(health);
    }

    /// <summary>
    /// Проверяет, что значение настроения находится в диапазоне от 0 до 100 включительно.
    /// </summary>
    /// <param name="mood">Значение настроения для проверки.</param>
    /// <returns>Успешный результат, если значение допустимо; в противном случае, результат сбоя с ошибкой выхода за пределы диапазона.</returns>
    private static Result ValidateMood(int mood)
    {
        if (mood < 0 || mood > 100)
        {
            return Result.Failure(PlayerError.MoodOutOfRange());
        }
        return Result.Success(mood);
    }

    /// <summary>
    /// Проверяет, что значение голода находится в диапазоне от 0 до 100 включительно.
    /// </summary>
    /// <param name="hunger">Значение голода для проверки.</param>
    /// <returns>Успешный результат, если значение допустимо; в противном случае, результат сбоя с ошибкой выхода за пределы диапазона.</returns>
    private static Result ValidateHunger(int hunger)
    {
        if (hunger < 0 || hunger > 100)
        {
            return Result.Failure(PlayerError.HungerOutOfRange());
        }
        return Result.Success(hunger);
    }

    /// <summary>
    /// Корректирует здоровье игрока на указанную величину, ограничивая результат значениями от 0 до 100.
    /// Вызывает событие StatsChanged, если значение здоровья изменяется.
    /// </summary>
    /// <param name="delta">Величина, на которую изменяется здоровье. Положительное значение увеличивает, отрицательное уменьшает.</param>
    public void ChangeHealth(int delta)
    {
        int prevValue = Health;
        int newValue = Health + delta;
        Health = Math.Clamp(newValue, 0, 100);

        if (Health != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет уровень голода игрока на указанную величину, ограничивая результат значениями от 0 до 100.
    /// Вызывает событие StatsChanged, если значение голода изменяется.
    /// </summary>
    /// <param name="delta">Величина, на которую изменяется уровень голода. Положительное значение увеличивает голод, отрицательное уменьшает.</param>
    public void ChangeHunger(int delta)
    {
        int prevValue = Hunger;
        int newValue = Hunger + delta;
        Hunger = Math.Clamp(newValue, 0, 100);

        if (Hunger != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Корректирует настроение игрока на указанную величину, ограничивая результат значениями от 0 до 100.
    /// Вызывает событие StatsChanged, если значение настроения изменяется.
    /// </summary>
    /// <param name="delta">Величина, на которую изменяется настроение.</param>
    public void ChangeMood(int delta)
    {
        int prevValue = Mood;
        int newValue = Mood + delta;
        Mood = Math.Clamp(newValue, 0, 100);

        if (Mood != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Изменяет количество карманных денег игрока на указанную величину, обеспечивая, чтобы оно не стало меньше нуля.
    /// Вызывает событие StatsChanged, если значение изменяется.
    /// </summary>
    /// <param name="delta">Величина, на которую увеличиваются или уменьшаются карманные деньги игрока.</param>
    public void ChangePocketMoney(decimal delta)
    {
        decimal prevValue = PocketMoney;
        PocketMoney = Math.Max(0, PocketMoney + delta);

        if (PocketMoney != prevValue)
            StatsChanged?.Invoke(this);
    }

    /// <summary>
    /// Обновляет статус жизни игрока и вызывает событие StatsChanged, если значение изменяется.
    /// </summary>
    /// <param name="isAlive">Новый статус жизни, который нужно установить для игрока.</param>
    public void ChangeIsAlive(bool isAlive)
    {
        bool prevValue = IsAlive;
        IsAlive = isAlive;

        if (IsAlive != prevValue)
            StatsChanged?.Invoke(this);
    }
}

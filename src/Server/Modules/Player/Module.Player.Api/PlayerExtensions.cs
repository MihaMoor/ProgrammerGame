using Server.Module.Player.GrpcContracts.V1;

namespace Server.Module.Player.Api;

internal static class PlayerExtensions
{
    /// <summary>
    /// Преобразует экземпляр <see cref="Domain.Player"/> в <see cref="PlayerDto"/> для использования в ответах API.
    /// </summary>
    /// <param name="stats">Доменная модель игрока для преобразования.</param>
    /// <returns><see cref="PlayerDto"/>, представляющий данные игрока.</returns>
    public static PlayerDto ToViewModel(this Domain.Player stats) =>
        new()
        {
            PlayerId = stats.PlayerId.ToString(),
            Name = stats.Name,
            Health = stats.Health,
            Hunger = stats.Hunger,
            Mood = stats.Mood,
            PocketMoney = MoneyConverter.ToMoney(stats.PocketMoney)
        };
}

public static class MoneyConverter
{
    private const string DefaultCurrency = "RUB";

    /// <summary>
    /// Преобразует десятичную денежную сумму в объект <see cref="Google.Type.Money"/>, разделяя значение на единицы (units) и нано-единицы (nanos)
    /// с обработкой отрицательных сумм в соответствии со спецификацией Money.
    /// </summary>
    /// <param name="amount">Денежная сумма для преобразования.</param>
    /// <param name="currencyCode">Код валюты по стандарту ISO 4217. По умолчанию "RUB".</param>
    /// <returns>Экземпляр <see cref="Google.Type.Money"/>, представляющий указанную сумму и валюту.</returns>
    public static Google.Type.Money ToMoney(decimal amount, string currencyCode = DefaultCurrency)
    {
        long units = (long)Math.Truncate(Math.Abs(amount));
        decimal fractionalPart = Math.Abs(amount) - units;
        int nanos = (int)(fractionalPart * 1_000_000_000);

        // Обработка отрицательных значений
        if (amount < 0)
        {
            if (nanos > 0)
            {
                units = -units - 1;
                nanos = 1_000_000_000 - nanos;
            }
            else
            {
                units = -units;
            }
        }

        return new Google.Type.Money
        {
            CurrencyCode = currencyCode,
            Units = units,
            Nanos = nanos
        };
    }

    /// <summary>
    /// Преобразует объект <see cref="Google.Type.Money"/> в десятичное представление денежной суммы.
    /// </summary>
    /// <param name="money">Экземпляр <see cref="Google.Type.Money"/> для преобразования. Если null, возвращает 0.</param>
    /// <returns>Десятичное представление денежной суммы с сохранением знака и дробной части.</returns>
    public static decimal FromMoney(Google.Type.Money money)
    {
        if (money == null)
            return 0m;

        decimal result = Math.Abs(money.Units) + (decimal)Math.Abs(money.Nanos) / 1_000_000_000;

        // Проверка знака
        if (money.Units < 0 || (money.Units == 0 && money.Nanos < 0))
        {
            result = -result;
        }

        return result;
    }
}

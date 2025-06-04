using Server.Module.Player.GrpcContracts.V1;

namespace Server.Module.Player.Api;

internal static class PlayerExtensions
{
    /// <summary>
        /// Converts a <see cref="Domain.Player"/> instance to a <see cref="PlayerDto"/> for API responses.
        /// </summary>
        /// <param name="stats">The player domain model to convert.</param>
        /// <returns>A <see cref="PlayerDto"/> representing the player's data.</returns>
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
    /// Converts a decimal monetary amount to a <see cref="Google.Type.Money"/> object, splitting the value into units and nanos and handling negative amounts according to the Money specification.
    /// </summary>
    /// <param name="amount">The monetary amount to convert.</param>
    /// <param name="currencyCode">The ISO 4217 currency code. Defaults to "RUB".</param>
    /// <returns>A <see cref="Google.Type.Money"/> instance representing the specified amount and currency.</returns>
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
    /// Converts a <see cref="Google.Type.Money"/> object to its decimal monetary value.
    /// </summary>
    /// <param name="money">The <see cref="Google.Type.Money"/> instance to convert. If null, returns 0.</param>
    /// <returns>The decimal representation of the monetary amount, preserving sign and fractional value.</returns>
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

using Server.Module.Player.GrpcContracts.V1;

namespace Server.Module.Player.Api;

internal static class PlayerExtensions
{
    public static PlayerDto ToViewModel(this Domain.Player stats) =>
        new()
        {
            PlayerId = stats.Id,
            Name = stats.Name,
            Health = stats.Health,
            Hunger = stats.Hunger,
            Mood = stats.Mood,
            PocketMoney = MoneyConverter.ToMoney(player.PocketMoney)
        };
}

public static class MoneyConverter
{
    private const string DefaultCurrency = "RUB";

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

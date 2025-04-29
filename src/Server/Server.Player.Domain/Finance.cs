namespace Server.Player.Domain;

public class Finance
{
    /// <summary>
    /// Карманные деньги
    /// </summary>
    public double PocketMoney { get; set; }
    /// <summary>
    /// Сберегательный счет в банке. Обычно это большой набор цифр.
    /// </summary>
    public string BankSavingsAccount { get; set; }
    /// <summary>
    /// Кредитный счет в банке. Обычно это большой набор цифр.
    /// </summary>
    public string BankCreditAccount { get; set; }
}
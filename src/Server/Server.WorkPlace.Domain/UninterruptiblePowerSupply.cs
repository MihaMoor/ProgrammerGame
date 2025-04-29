namespace Server.WorkPlace.Domain;

public class UninterruptiblePowerSupply
{
    public string Model { get; set; }

    /// <summary>
    /// Уровень шума вентилятора, dB
    /// </summary>
    public float FanNoiseLevel { get; set; }

    /// <summary>
    /// Активная мощность, которой может обеспечивать потребителей
    /// </summary>
    public uint ActivePower { get; set; }

    /// <summary>
    /// Время зарядки
    /// </summary>
    public uint ChargingTime { get; set; }

    /// <summary>
    /// Время автономной работы
    /// </summary>
    public uint BatteryLife { get; set; }
}

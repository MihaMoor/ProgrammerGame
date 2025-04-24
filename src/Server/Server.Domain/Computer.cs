namespace Server.Domain;

public enum Socket
{
    LGA1851,
    Core5,
    Core7,
}

public enum MemoryType
{
    DDR3,
    DDR4,
    DDR5,
}

public class CentralProcessingUnit
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }
    /// <summary>
    /// Частота, Гц
    /// </summary>
    public long Frequency { get; set; }
    /// <summary>
    /// Сокет
    /// </summary>
    public Socket Socket { get; set; }
    /// <summary>
    /// Количество ядер, шт
    /// </summary>
    public uint CoresCount { get; set; }
    /// <summary>
    /// Размер L1 кэша, Kb
    /// </summary>
    public uint L1CashSize { get; set; }
    /// <summary>
    /// Размер L2 кэша, Kb
    /// </summary>
    public uint L2CashSize { get; set; }
    /// <summary>
    /// Размер L3 кэша, Kb
    /// </summary>
    public uint L3CashSize { get; set; }
    /// <summary>
    /// Количество потоков, шт
    /// </summary>
    public uint ThreadsCount { get; set; }
    /// <summary>
    /// Совместимость с типом оперативной памяти
    /// </summary>
    public MemoryType MemoryType { get; set; }
    /// <summary>
    /// Максимально поддерживаемый объем памяти, Gb
    /// </summary>
    public uint MaxSupportedMemoryCapacity { get; set; }
    /// <summary>
    /// Количество каналов, шт
    /// </summary>
    public uint ChannelsCount { get; set; }
    /// <summary>
    /// Максимально поддерживаемая частота оперативной памяти, Mgh
    /// </summary>
    public uint MaxRamFrequency { get; set; }
    /// <summary>
    /// Тепловыделение, Вт
    /// </summary>
    public uint TDP { get; set; }

    public long MegaHertz()
        => Frequency / 1_000_000;

    public long GigaHertz()
        => MegaHertz() / 1_000;

    public float CpuPower()
        => GigaHertz() * CoresCount * ThreadsCount;
}

public class Motherboard
{
    public string Model { get; set; }
    public Socket CpuSocket { get; set; }
}

public class Case { }

public class NetworkCard { }

public class PowerSupply { }

public class HardDrive { }

public class Memory { }

public class Cooling { }

public class GraphicsCard
{
}

public class Computer
{
    public Case? Case { get; set; }
    public Motherboard? Motherboard { get; set; }
    public NetworkCard? NetworkCard { get; set; }
    public PowerSupply? PowerSupply { get; set; }
    public HardDrive? HardDrive { get; set; }
    public CentralProcessingUnit? Cpu { get; set; }
    public Memory? Ram { get; set; }
    public Cooling? Cooling { get; set; }
    public List<GraphicsCard>? GraphicsCards { get; private set; }
    public bool CanReplaceGraphicsCard { get; private set; }
    public bool IsReady =>
        // Вообще, тут сложная логика проверки совместимости интерфейсов
        // Например, совпадение сокетов материнки и ЦПУ
        Motherboard != null &&
        NetworkCard != null &&
        PowerSupply != null &&
        HardDrive != null &&
        Cpu != null &&
        Ram != null &&
        Cooling != null &&
        GraphicsCards != null;
}
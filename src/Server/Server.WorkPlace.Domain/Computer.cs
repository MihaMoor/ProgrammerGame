﻿namespace Server.WorkPlace.Domain;

public enum Socket
{
    Lga1150,
    Lga1155,
    Lga1156,
    Lga1200,
    Lga1366,
    Lga1700,
    Lga1851,
}

public enum RamType
{
    Ddr3,
    Ddr4,
    Ddr5,
}

public enum MotherboardFormFactor
{
    Atx,
    MicroAtx,
    MiniAtx,
    MiniItx,
}

public enum InterfaceType
{
    UsbTypeA,
    UsbTypeC,
    Usb20,
    Usb30,
    Usb32Gen1,
    Usb32Gen2,
    DisplayPort,
    Hdmi,
    Vga,
    M2,
    Sata,
    PciExpress_x1,
    PciExpress20,
    PciExpress30_x16,
    PciExpress40_x4,
    PciExpress50_x16,
    Rj45,
    Wifi1,
    Wifi2,
    Wifi3,
    Wifi3e,
    Wifi4,
    Wifi5,
    Wifi6,
    Wifi6e,
    Wifi7,
    Aux,
}

public enum CoolingType
{
    /// <summary>
    /// Водяное
    /// </summary>
    Liquid,

    /// <summary>
    /// Воздушное
    /// </summary>
    Air,
}

public enum HardDriveType
{
    Hdd,
    Ssd,
}

public enum GraphicsCardMemoryType
{
    Ddr3,
    Ddr4,
    Gddr3,
    Gddr4,
    Gddr5,
    Gddr6,
    Gddr6X,
    Gddr7,
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
    public RamType RamType { get; set; }

    /// <summary>
    /// Максимально поддерживаемый объем памяти, Gb
    /// </summary>
    public uint MaxSupportedRamCapacity { get; set; }

    /// <summary>
    /// Количество каналов памяти, шт
    /// </summary>
    public uint RamChannelsCount { get; set; }

    /// <summary>
    /// Максимально поддерживаемая частота оперативной памяти, Mgh
    /// </summary>
    public uint MaxRamFrequency { get; set; }

    /// <summary>
    /// Тепловыделение, Вт
    /// </summary>
    public uint Tdp { get; set; }

    /// <summary>
    /// Потребляемая мощность
    /// </summary>
    public float PowerConsumption { get; set; }

    public long MegaHertz() => Frequency / 1_000_000;

    public long GigaHertz() => MegaHertz() / 1_000;

    public float CpuPower() => GigaHertz() * CoresCount * ThreadsCount;
}

public class ConnectionInterface
{
    /// <summary>
    /// Тип интерфеса
    /// </summary>
    public InterfaceType InterfaceType { get; set; }

    /// <summary>
    /// Пропускная способность
    /// </summary>
    public long Speed { get; set; }

    /// <summary>
    /// Потребляемая мощность
    /// </summary>
    public float PowerConsumption { get; set; }
}

public class Motherboard
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Сокет подключения ЦПУ
    /// </summary>
    public Socket CpuSocket { get; set; }

    /// <summary>
    /// Тип поддерживаемой памяти
    /// </summary>
    public RamType RamType { get; set; }

    /// <summary>
    /// Форм-фактор платы
    /// </summary>
    public MotherboardFormFactor FormFactor { get; set; }

    /// <summary>
    /// Количество слотов под память
    /// </summary>
    public uint RamSlotsCount { get; set; }

    /// <summary>
    /// Количество каналов памяти
    /// </summary>
    public uint RamChannelsCount { get; set; }

    /// <summary>
    /// Максимально поддерживаемый объем памяти
    /// </summary>
    public uint MaxSupportedRamCapacity { get; set; }

    /// <summary>
    /// Максимальная частота памяти
    /// </summary>
    public uint MaxRamFrequency { get; set; }

    /// <summary>
    /// Список интерфейсов подключения как внешних, так и внутренних.
    /// </summary>
    public List<ConnectionInterface> Interfaces { get; private set; }

    /// <summary>
    /// Встроенная сетевая карта
    /// </summary>
    public NetworkCard NetworkCard { get; set; }

    /// <summary>
    /// Количество пинов для подключения питания
    /// </summary>
    public uint PowerPinsCount { get; set; }

    /// <summary>
    /// Потребляемая мощность
    /// </summary>
    public float PowerConsumption { get; set; }
}

public class Case
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Форм-фактор поддерживаемых материнских плат
    /// </summary>
    public MotherboardFormFactor FormFactor { get; set; }

    /// <summary>
    /// Потребляемая мощность
    /// </summary>
    public float PowerConsumption { get; set; }
}

public class NetworkCard
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Интерфейс подключения к материнской плате
    /// </summary>
    public InterfaceType ConnectionToMotherboardInterfaceType { get; set; }

    /// <summary>
    /// Интерфейсы подключения к интернету
    /// </summary>
    public List<InterfaceType> InterfaceTypes { get; private set; }

    /// <summary>
    /// Потребляемая мощность
    /// </summary>
    public float PowerConsumption { get; set; }
}

public class PowerSupply
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Мощность
    /// </summary>
    public long Power { get; set; }

    /// <summary>
    /// Форм-фактор
    /// </summary>
    public MotherboardFormFactor FormFactor { get; set; }

    /// <summary>
    /// Колиество пинов для подключения материнской платы
    /// </summary>
    public uint MotherboardPinsCount { get; set; }

    /// <summary>
    /// Количество пинов для подключения видеокарты
    /// </summary>
    public uint GraphicsCardPinsCount { get; set; }

    /// <summary>
    /// Наработка на отказ, количество часов
    /// </summary>
    public long Mtbf { get; set; }

    /// <summary>
    /// Производительность (КПД)
    /// </summary>
    public uint Performance { get; set; }

    /// <summary>
    /// Уровень шума вентилятора, dB
    /// </summary>
    public float FanNoiseLevel { get; set; }
}

public class HardDrive
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Потребляемая мощность
    /// </summary>
    public float PowerConsumption { get; set; }

    /// <summary>
    /// Тип диска
    /// </summary>
    public HardDriveType HardDriveType { get; set; }

    /// <summary>
    /// Объем, Gb
    /// </summary>
    public uint Capacity { get; set; }

    /// <summary>
    /// Тип подключения
    /// </summary>
    public InterfaceType ConnectionInterface { get; set; }

    /// <summary>
    /// Максимальная скорость чтения, Mb/s
    /// </summary>
    public uint MaxReadSpeed { get; set; }

    /// <summary>
    /// Максимальная скорость записи, Mb/s
    /// </summary>
    public uint MaxWriteSpeed { get; set; }

    /// <summary>
    /// Ресурс, Tb
    /// </summary>
    public uint Tbw { get; set; }
}

public class Ram
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// ОБъем, Mb
    /// </summary>
    public long Capacity { get; set; }

    /// <summary>
    /// Частота, Gh
    /// </summary>
    public long Frequency { get; set; }

    /// <summary>
    /// Тип памяти
    /// </summary>
    public RamType RamType { get; set; }

    /// <summary>
    /// Пропускная способность, Mb/s
    /// </summary>
    public long Throughput { get; set; }

    /// <summary>
    /// Потребляемая мощность
    /// </summary>
    public float PowerConsumption { get; set; }
}

public class Cooling
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Тип охлаждения
    /// </summary>
    public CoolingType CoolingType { get; set; }

    /// <summary>
    /// Уровень шума вентилятора, dB
    /// </summary>
    public float FanNoiseLevel { get; set; }

    /// <summary>
    /// Максимальное тепловыделение процессора, Wt
    /// </summary>
    public uint MaxCpuHeatDissipation { get; set; }

    /// <summary>
    /// Список совместимых сокетов
    /// </summary>
    public List<Socket> CompatibleSockets { get; private set; }

    /// <summary>
    /// Потребляемая мощность, Wt
    /// </summary>
    public float PowerConsumption { get; set; }
}

public class GraphicsCard
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Потребляемая мощность, Wt
    /// </summary>
    public float PowerConsumption { get; set; }

    /// <summary>
    /// Тип подключения к материнской плате
    /// </summary>
    public InterfaceType MotherboardInterfaceType { get; set; }

    /// <summary>
    /// Частота, Gh
    /// </summary>
    public long Frequency { get; set; }

    /// <summary>
    /// Максимальное разрешение монитора
    /// </summary>
    public (uint, uint) MaxResolution { get; set; }

    /// <summary>
    /// Число процессоров (ядер)
    /// </summary>
    public uint ProcessorsCount { get; set; }

    /// <summary>
    /// Объем, Gb
    /// </summary>
    public uint Capacity { get; set; }

    /// <summary>
    /// Тип памяти
    /// </summary>
    public GraphicsCardMemoryType GraphicsCardMemoryType { get; set; }

    /// <summary>
    /// Разрядность шины видеопамяти, bit
    /// </summary>
    public uint BusWidth { get; set; }

    /// <summary>
    /// Интерфейсы подключения мониторов
    /// </summary>
    public List<InterfaceType> InterfaceTypes { get; private set; }

    /// <summary>
    /// Количество пинов доп питания
    /// </summary>
    public uint PowerPinsCount { get; set; }

    /// <summary>
    /// Уровень шума вентилятора, dB
    /// </summary>
    public float FanNoiseLevel { get; set; }
}

public class Computer
{
    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Корпус
    /// </summary>
    public Case? Case { get; set; }

    /// <summary>
    /// Материнка
    /// </summary>
    public Motherboard? Motherboard { get; set; }

    /// <summary>
    /// Сетевая карта
    /// </summary>
    public NetworkCard? NetworkCard { get; set; }

    /// <summary>
    /// Блок питания
    /// </summary>
    public PowerSupply? PowerSupply { get; set; }

    /// <summary>
    /// Жесткие диски
    /// </summary>
    public List<HardDrive>? HardDrive { get; set; }

    /// <summary>
    /// ЦПУ
    /// </summary>
    public CentralProcessingUnit? Cpu { get; set; }

    /// <summary>
    /// Память
    /// </summary>
    public Ram? Ram { get; set; }

    /// <summary>
    /// Система охлаждения
    /// </summary>
    public Cooling? Cooling { get; set; }

    /// <summary>
    /// Видюхи
    /// </summary>
    public List<GraphicsCard>? GraphicsCards { get; private set; }

    /// <summary>
    /// Возможность заменить видеокарту
    /// </summary>
    public bool CanReplaceGraphicsCard { get; private set; }
    public bool IsReady =>
        // Вообще, тут сложная логика проверки совместимости интерфейсов
        // Например, совпадение сокетов материнки и ЦПУ
        Motherboard != null
        && NetworkCard != null
        && PowerSupply != null
        && HardDrive != null
        && Cpu != null
        && Ram != null
        && Cooling != null
        && GraphicsCards != null;

    public event Action<bool> ComputerReadyEvent;
}

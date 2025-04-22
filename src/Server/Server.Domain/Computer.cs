using System;

namespace Server.Domain;

public interface IComputer
{
    public float CpuPower()
        => CentralProcessingUnit.Hertz * CentralProcessingUnit.Multiplier;
}

public abstract class Computer
{
    public float CpuPower()
        => GetCentralProcessingUnitHertz() * 100;

    protected abstract float GetCentralProcessingUnitHertz();
}

public class Desktop : Computer
{
    public Case? Case { get; set; }
    public Motherboard? Motherboard { get; set; }
    public NetworkCard? NetworkCard { get; set; }
    public PowerSupply? PowerSupply { get; set; }
    public HardDrive? HardDrive { get; set; }
    public CentralProcessingUnit? Cpu { get; set; }
    public Memory? Ram { get; set; }
    public Cooling? Cooling { get; set; }
    public DiscreteGraphicsCard? DiscreteGraphicsCard { get; set; }

    protected override float GetCentralProcessingUnitHertz()
        => Cpu.Hertz;
}

public class Laptop : IComputer
{
    public string Name { get; set; }
    public Motherboard Motherboard { get; private set; }
    public NetworkCard NetworkCard { get; private set; }
    public PowerSupply PowerSupply { get; private set; }
    public HardDrive HardDrive { get; set; }
    public CentralProcessingUnit Cpu { get; set; }
    public Memory Ram { get; set; }
    public Cooling Cooling { get; private set; }
    public DiscreteGraphicsCard? DiscreteGraphicsCard { get; set; }
    public BuiltInGraphicsCard BuiltInGraphicsCard { get; private set; }

    protected override float GetCentralProcessingUnitHertz()
        => Cpu.Hertz;
}
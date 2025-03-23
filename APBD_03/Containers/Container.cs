namespace APBD_03.Containers;

public abstract class Container
{
    private const string ContainerPrefix = "KON";
    private static int _nextContainer = 1;
    
    
    protected Container(
        double height,
        double depth,
        double tareWeight,
        char containerType,
        double maxPayLoad)

    {
        Height = height > 0 ? height : throw new AggregateException("Height must be greater than 0");
        Depth = depth > 0 ? depth : throw new AggregateException("Depth must be greater than 0");
        TareWeight = tareWeight > 0 ? tareWeight : throw new AggregateException("TareWeight must be greater than 0");
        MaxPayLoad = maxPayLoad > 0 ? maxPayLoad : throw new AggregateException("MaxPayLoad must be greater than 0");
        SerialNumber = $"{ContainerPrefix}{containerType}-{_nextContainer++}";
    }
    
    public double Height { get; }
    public double Depth { get; }
    public double TareWeight { get; }
    public string SerialNumber { get; }
    
    public double MaxPayLoad { get; }
    
    public double CargoWeight { get;  protected set; }

    public virtual void LoadCargo(double mass)
    {
        if (mass <= 0) throw new AggregateException("Mass must be greater than 0");

        if (!CanLoadCargo(mass)) throw new OverflowException();
        
        CargoWeight += mass;
    }
    
    protected virtual void EmptyCargo() => CargoWeight = 0;
    
    protected virtual bool CanLoadCargo(double mass) => CargoWeight + mass <= MaxPayLoad;

    public virtual void printContainerInfo()
    {
        Console.WriteLine($"Container Information:");
        Console.WriteLine($"- Serial Number: {SerialNumber}");
        Console.WriteLine($"- Type: {GetType().Name}");
        Console.WriteLine($"- Dimensions: {Height}m x {Depth}m");
        Console.WriteLine($"- Tare Weight: {TareWeight} tons");
        Console.WriteLine($"- Max Payload: {MaxPayLoad} tons");
        Console.WriteLine($"- Current Cargo Weight: {CargoWeight} tons");
    }
}
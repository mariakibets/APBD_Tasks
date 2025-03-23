namespace APBD_03.Containers;

public class LiquidContainers : HazardContainer
{
    private readonly double _allowedMaxPayload;
    
    public LiquidContainers(
        double height,
        double depth,
        double tareWeight,
        double maxPayload,
        bool storesHazCargo) : base(height, depth, tareWeight, 'L', maxPayload )
    {
        StoresHazCargo = storesHazCargo;
        var limitCapacity = StoresHazCargo ? 0.5 : 0.9;
        _allowedMaxPayload = maxPayload * limitCapacity;
    }
    
    public bool StoresHazCargo { get; }

    protected override bool CanLoadCargo(double mass) => CargoWeight + mass <= _allowedMaxPayload;
}
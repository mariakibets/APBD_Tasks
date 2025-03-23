using APBD_03.Interfaces;

namespace APBD_03.Containers;

public class GasContainer : HazardContainer, IHazardNotifier
{
    public GasContainer(
        double height,
        double depth,
        double tareWeight,
        double maxPayLoad,
        double pressure)
        : base(height, depth, tareWeight, 'G', maxPayLoad)
    {
        Pressure = pressure > 0 ? pressure : throw new ArgumentOutOfRangeException("Presuure cna't be 0");
    }

    public double Pressure { get; }
    
    protected override void EmptyCargo() => CargoWeight *= 0.05;

}
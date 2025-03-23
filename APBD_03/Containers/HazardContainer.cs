using APBD_03.Interfaces;

namespace APBD_03.Containers;

public class HazardContainer : Container, IHazardNotifier
{
    public HazardContainer(
        double height,
        double depth,
        double tareWeight,
        char containerType,
        double maxPayLoad)
        : base(height, depth, tareWeight, containerType, maxPayLoad)
    {
    }

    public override void LoadCargo(double mass)
    {
        if(mass <= 0) throw new ArgumentException("Invalid amount of mass, must be greater than zero.");

        if (!CanLoadCargo(mass))
        {
            Notify("Container is full");
            throw new OverflowException();
        }
        CargoWeight += mass;
    }



    public void Notify(string message)
    {
            Console.WriteLine($"Hazardous situation occured for container {SerialNumber}: " +
                              $"\n Message: {message}");
    }
}
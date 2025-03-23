using APBD_03.Containers;

namespace APBD_03.ContainerShips;

public class ContainerShip
{
    private List<Container> _containers = new List<Container>();
    private int _currentContainerCount = 0;
    private double _currentContainerWeight = 0;

    public ContainerShip(
        string name,
        int maxContainerAmount,
        double maxSpeed,
        double maxWeight
    )
    {
        Name = name;
        MaxAmount = maxContainerAmount > 0
            ? maxContainerAmount
            : throw new ArgumentOutOfRangeException("The amount can't be 0 or negative.");
        MaxSpeed = maxSpeed > 0 ? maxSpeed : throw new ArgumentOutOfRangeException("The speed can't be 0 or negative.");
        MaxWeight = maxWeight > 0
            ? maxWeight
            : throw new ArgumentOutOfRangeException("The weight can't be 0 or negative.");
        Containers = new List<Container>();
        Count = _currentContainerCount;
        CurrentWeight = _currentContainerWeight;
    }

    public string Name { get; }
    public double MaxAmount { get; }
    public double MaxSpeed { get; }
    public double MaxWeight { get; }
    public double CurrentWeight { get; }
    public int Count { get; }
    public List<Container> Containers { get; }
    
    
    public void addContainer(Container container)
    {
        if (_currentContainerCount >= MaxAmount) 
            throw new OverflowException("The ship is full due to container count.");
        
        var totalWeight = _currentContainerWeight + container.TareWeight + container.CargoWeight;
        if (totalWeight > MaxWeight) 
            throw new OverflowException("The ship is full due to weight limits.");
        
        Containers.Add(container);
        _currentContainerCount++;
        _currentContainerWeight = totalWeight;
    }


    public void removeContainer(string serialNumber)
    {
        Container container = null;
        foreach (var c in Containers)
        {
            if (c.SerialNumber == serialNumber) container = c;
        }
        if (container != null) Containers.Remove(container);
        else throw new InvalidOperationException("Container not found!");
    }

    public void ReplaceContainer(string serialNumber, Container newContainer)
    {
        removeContainer(serialNumber);
        addContainer(newContainer);
    }
    
    public void transferContainer(ContainerShip targetShip, string serialNumber)
    {
        Container container = null;
        foreach (var c in Containers)
        {
            if (c.SerialNumber == serialNumber) container = c;
        }
        if (container == null) throw new InvalidOperationException("Container not found for transfer!");
        removeContainer(serialNumber);
        targetShip.addContainer(container);
    }
    
    public void printShipInfo()
    {
        Console.WriteLine($"Ship {Name} - Max Speed: {MaxSpeed} knots, Capacity: {Containers.Count}/{MaxAmount}," +
                          $" Weight: {_currentContainerWeight}/{MaxWeight} tons");
        foreach (var container in Containers)
            Console.WriteLine($"  - {container}");
    }


}
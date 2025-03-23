using APBD_03.Containers;
using APBD_03.ContainerShips;
using APBD_03.Enums;

class Program
{
    static void Main()
    {
        ContainerShip ship1 = new("Poseidon", 30, 10, 100000);
        ContainerShip ship2 = new("Titanic II", 25, 15, 710000);
        
        GasContainer gasContainer = new(2.5, 2.0, 500, 2000, 10);
        RefContainer refContainer = new(3.0, 2.5, 600, 1500, Products.IceCream, -35);
        LiquidContainers liquidContainer = new(5.3, 4.0, 400, 2000, true);
        
        gasContainer.LoadCargo(1000);
        refContainer.LoadCargo(800);
        
        Console.WriteLine("Container Info Before Adding to Ships:");
        gasContainer.printContainerInfo();
        refContainer.printContainerInfo();
        liquidContainer.printContainerInfo();
        
        ship1.addContainer(gasContainer);
        ship1.addContainer(refContainer);
        ship1.printShipInfo();
        
        ship2.addContainer(liquidContainer);
        ship2.printShipInfo();
        
        Console.WriteLine("\nRemoving the gas container (KONG-1)");
        ship1.removeContainer(gasContainer.SerialNumber);
        ship1.printShipInfo();
        
        Console.WriteLine("\nTransferring");
        ship1.transferContainer(ship2, refContainer.SerialNumber);
        
        Console.WriteLine("\nAfter Transfer:");
        ship1.printShipInfo();
        ship2.printShipInfo();
    }
}
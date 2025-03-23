using APBD_03.Enums;

namespace APBD_03.Containers;

public class RefContainer : Container
{
    public RefContainer(
        double height,
        double depth,
        double tareWeight,
        double maxPayload,
        Products productType,
        double temperature
    ) : base(height, depth, tareWeight, 'R', maxPayload)
    {
        if (TemperatureValidator.isValidTemperature(temperature, productType))
        {
            Temperature = temperature;
            ProductType = productType;
        }
        else
        {
            throw new Exception($"Invalid temperature: {temperature}");
        }
    }
    
    public Products ProductType { get; }
    public double Temperature { get; }
    
    
}
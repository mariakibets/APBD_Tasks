using APBD_03.Enums;

namespace APBD_03;

public class TemperatureValidator
{

    public static double getTheProductsTemperature(Products products)
    {
        var temperature = products switch
        {
            Products.Fish => 10.6,
            Products.Meat => 15.6,
            Products.IceCream => -30,
            Products.FrozenVegies => -15,
            Products.Pineapple => 10,
            Products.SourCream => 8.8,
            Products.Cake => 12.8,
            Products.Eggs => 20,
            Products.Milk => 15.8,
            _ => throw new ArgumentOutOfRangeException("Invalid product type")
        };
        return temperature;
    }


    public static bool isValidTemperature(double currentTemperature, Products product)
    {
        var allowedTemperature = getTheProductsTemperature(product);
        return allowedTemperature >= currentTemperature;
    }
}
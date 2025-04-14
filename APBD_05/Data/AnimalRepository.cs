using APBD_05.Models;

namespace APBD_05.Data;

public static class AnimalRepository
{
    public static List<Animal> animals =
    [
        new() { Id = 1, Name = "Lion", Category = "Mammal", Weight = 190, FurColor = "Golden" },
        new() { Id = 2, Name = "Polar Bear", Category = "Mammal", Weight = 450, FurColor = "White" },
        new() {Id = 3, Name = "Red Fox", Category = "Mammal", Weight = 6, FurColor = "Red" },
        new() {Id = 4, Name = "Alpaca", Category = "Mammal", Weight = 65, FurColor = "Mixed" },
        new() {Id = 5, Name = "Capybara :0", Category = "Mammal", Weight = 50, FurColor = "Brown" },
        new(){Id = 6, Name = "Chinchilla", Category = "Mammal", Weight = 0.6, FurColor = "Grey" },
    ];

}
using APBD_05.Models;

namespace APBD_05.Data;

public static class VisitsRepository
{
    public static List<Visits> visits =
    [
        new() { Id = 1, DateOfVisit = new DateTime(2024, 10, 5), AnimalId = 1, Describtion = "Routine check-up", Price = 45.50 },
        new() { Id = 2, DateOfVisit = new DateTime(2024, 11, 15), AnimalId = 2, Describtion = "Vaccination", Price = 30.00 },
        new() { Id = 3, DateOfVisit = new DateTime(2024, 12, 20), AnimalId = 3, Describtion = "Injury treatment", Price = 75.25 },
        new() { Id = 4, DateOfVisit = new DateTime(2025, 1, 10), AnimalId = 4, Describtion = "Dental cleaning", Price = 60.00 },
        new() { Id = 5, DateOfVisit = new DateTime(2025, 2, 3), AnimalId = 5, Describtion = "Follow-up visit", Price = 40.00 },
    ];

}
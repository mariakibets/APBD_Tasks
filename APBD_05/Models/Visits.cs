namespace APBD_05.Models;

public class Visits
{
    public required int Id { get; set; }
    public required DateTime DateOfVisit { get; set; }
    public required int AnimalId { get; set; }
    public required string Describtion { get; set; }
    public required double Price { get; set; }
}
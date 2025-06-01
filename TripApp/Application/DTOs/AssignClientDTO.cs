namespace TripApp.Application.DTOs;

public class AssignClientDTO
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Pesel { get; set; } = string.Empty;
    
    public int IdTrip { get; set; } = 0;
    
    public string TripName { get; set; } = string.Empty;
    
    public DateTime PaymentDate { get; set; }
}
namespace CodeFirst.DTO;

public class PatientResponse
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public List<PrescriptionResponse> Prescriptions { get; set; }
    
}
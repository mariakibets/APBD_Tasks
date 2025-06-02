namespace CodeFirst.DTO;

public class PerscriptionDTO
{
    public PatientDTO Patient { get; set; }
    
    // public DoctorDTO Doctor { get; set; }
    
    public List<MedicamentsDTO> medicaments { get; set; }
    
    public DateTime Date { get; set; }
    
    public DateTime DueDate { get; set; }
}
namespace CodeFirst.DTO;

public class PrescriptionResponse
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    
    public List<MedicamentsResponse> Medicaments { get; set; }
    
    public DoctorResponse Doctor { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public Patient Patient { get; set; } = null!;
    public int IdDoctor { get; set; }
    public Doctor Doctor { get; set; } = null!;
    
    public ICollection<Prescription_Medicament> Perscription_Medicaments { get; set; } = new List<Prescription_Medicament>();
}
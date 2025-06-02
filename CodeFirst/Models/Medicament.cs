using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string Type { get; set; }
    
    public ICollection<Prescription_Medicament> Perscription_Medicaments { get; set; } = new List<Prescription_Medicament>();
}
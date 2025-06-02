using CodeFirst.DAL;
using CodeFirst.Models;
using CodeFirst.Repositories.Inetrfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly HospitalContext _context;

    public PatientRepository(HospitalContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetPatientWithPrescriptionsAsync(int patientId)
    {
        return await _context.Patients
            .Include(p => p.Prescriptions.OrderBy(pr => pr.DueDate))
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Perscription_Medicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);
    }
    
}
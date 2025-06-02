using CodeFirst.DAL;
using CodeFirst.Models;
using CodeFirst.Repositories.Inetrfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Repositories;

public class PrescriptionRepository : IPresciptionRepository
{
    private readonly HospitalContext _context;

    public PrescriptionRepository(HospitalContext context)
    {
        _context = context;
    }
    
    public async Task<bool> PatientExistsAsync(int patientId)
    {
        return await _context.Patients.AnyAsync(p => p.IdPatient == patientId);
    }

    public async Task<bool> DoctorExistsAsync(int doctorId)
    {
        return await _context.Doctors.AnyAsync(doctor => doctor.IdDoctor == doctorId);
    }

    public async Task<bool> MedicamentExistsAsync(int medicamentId)
    {
        return await _context.Medicaments.AnyAsync(m => m.IdMedicament == medicamentId);
    }

    public async Task AddPatientAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
    }

    public async Task<int> AddPrescriptionAsync(Prescription prescription)
    {
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
        return prescription.IdPrescription;
    }

    public async Task AddPrescriptionMedicamentAsync(Prescription_Medicament pm)
    {
        _context.PrescriptionMedicaments.Add(pm);
        await _context.SaveChangesAsync();
    }

}
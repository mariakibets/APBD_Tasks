using CodeFirst.Models;

namespace CodeFirst.Repositories.Inetrfaces;

public interface IPresciptionRepository
{
    Task<bool> PatientExistsAsync(int patientId);
    Task<bool> MedicamentExistsAsync(int medicamentId);
    Task AddPatientAsync(Patient patient);
    Task<int> AddPrescriptionAsync(Prescription prescription);
    Task AddPrescriptionMedicamentAsync(Prescription_Medicament pm);

    Task<bool> DoctorExistsAsync(int doctorId);
}
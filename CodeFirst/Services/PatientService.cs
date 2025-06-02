using CodeFirst.DTO;
using CodeFirst.Repositories.Inetrfaces;
using CodeFirst.Services.Interfaces;

namespace CodeFirst.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService (IPatientRepository repository)
    {
        _repository = repository;
    }
    
    
    public async Task<PatientResponse?> GetPatientDetailsAsync(int patientId)
    {
        var patient = await _repository.GetPatientWithPrescriptionsAsync(patientId);

        if (patient == null) return null;

        return new PatientResponse
        {
            Id = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionResponse
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorResponse
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                    },
                    Medicaments = p.Perscription_Medicaments.Select(pm => new MedicamentsResponse
                    {
                        IdMedicament = pm.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Details
                    }).ToList()
                }).ToList()
        };
    }

}
using CodeFirst.DTO;
using CodeFirst.Models;
using CodeFirst.Repositories.Inetrfaces;
using CodeFirst.Services.Interfaces;

namespace CodeFirst.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPresciptionRepository _repository;

    public PrescriptionService(IPresciptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> IssuePrescriptionAsync(PerscriptionDTO request)
    {
        if (request.DueDate < request.Date)
        {
            return "DueDate cannot be earlier than Date.";
        }
        
        if (request.medicaments.Count > 10)
        {
            return "A prescription cannot include more than 10 medicaments.";
        }
        
        foreach (var med in request.medicaments)
        {
            if (!await _repository.MedicamentExistsAsync(med.IdMedicament))
            {
                return $"Medicament with ID {med.IdMedicament} does not exist.";
            }
        }
        
        bool patientExists = await _repository.PatientExistsAsync(request.Patient.Id);
        if (!patientExists)
        {
            var patient = new Patient()
            {
                IdPatient = request.Patient.Id,
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                DateOfBirth = request.Patient.DateOfBirth
            };

            await _repository.AddPatientAsync(patient);
        }
        
        // bool doctorExists = await _repository.DoctorExistsAsync(request.Doctor.IdDoctor);
        // if (!doctorExists)
        // {
        //     return $"Doctor with ID {request.Doctor.IdDoctor} does not exist.";
        // }
        
        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdPatient = request.Patient.Id,
            // IdDoctor = request.Doctor.IdDoctor
        };

        int prescriptionId = await _repository.AddPrescriptionAsync(prescription);

        foreach (var med in request.medicaments)
        {
            var pm = new Prescription_Medicament
            {
                IdPrescription = prescriptionId,
                IdMedicament = med.IdMedicament,
                Dose = med.Dose,
                Details = med.Description
            };

            await _repository.AddPrescriptionMedicamentAsync(pm);
        }

        return "Prescription issued successfully.";
    }
}
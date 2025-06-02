using CodeFirst.DTO;
using CodeFirst.Models;

namespace CodeFirst.Services.Interfaces;

public interface IPatientService
{
    Task<PatientResponse?> GetPatientDetailsAsync(int patientId);
}
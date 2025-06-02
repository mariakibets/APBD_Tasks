using CodeFirst.Models;

namespace CodeFirst.Repositories.Inetrfaces;

public interface IPatientRepository
{
    public Task<Patient?> GetPatientWithPrescriptionsAsync(int patientId);
}
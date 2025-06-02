using CodeFirst.DTO;

namespace CodeFirst.Services.Interfaces;

public interface IPrescriptionService
{
    Task<string> IssuePrescriptionAsync(PerscriptionDTO request);
}
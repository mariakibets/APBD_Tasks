using TripApp.Core.Models;

namespace TripApp.Application.Repository;

public interface ITripRepository
{
    Task<PaginatedResult<Core.Models.Trip>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10);
    Task<List<Core.Models.Trip>> GetAllTripsAsync();
    
    Task<Core.Models.Trip?> GetTripByIdAsync(int idTrip);
    Task<bool> ClientExistsByPeselAsync(string pesel);
    Task<bool> IsClientRegisteredToTripAsync(int idTrip, string pesel);
    Task AddClientAsync(Core.Models.Client client);
    Task AddClientTripAsync(Core.Models.ClientTrip clientTrip);
    Task SaveChangesAsync();
}
using Microsoft.EntityFrameworkCore;
using TripApp.Application.Repository;
using TripApp.Core.Models;

namespace TripApp.Infrastructure.Repository;

public class TripRepository(TripContext tripDbContext) : ITripRepository
{
    public async Task<PaginatedResult<Core.Models.Trip>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10)
    {
        var tripsQuery = tripDbContext.Trips
            .Include(e => e.ClientTrips).ThenInclude(e => e.IdClientNavigation)
            .Include(e => e.IdCountries)
            .OrderByDescending(e => e.DateFrom);

        var tripsCount = await tripsQuery.CountAsync();
        var totalPages = tripsCount / pageSize;
        var trips = await tripsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<Core.Models.Trip>
        {
            PageSize = pageSize,
            PageNum = page,
            AllPages = totalPages,
            Data = trips
        };
    }

    public async Task<List<Core.Models.Trip>> GetAllTripsAsync()
    {
        return await tripDbContext.Trips
            .Include(e => e.ClientTrips).ThenInclude(e => e.IdClientNavigation)
            .Include(e => e.IdCountries)
            .OrderBy(e => e.DateFrom)
            .ToListAsync();
    }
    
    
    public async Task<Core.Models.Trip?> GetTripByIdAsync(int idTrip)
    {
        return await tripDbContext.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .FirstOrDefaultAsync(t => t.IdTrip == idTrip);
    }

    public async Task<bool> ClientExistsByPeselAsync(string pesel)
    {
        return await tripDbContext.Clients.AnyAsync(c => c.Pesel == pesel);
    }

    public async Task<bool> IsClientRegisteredToTripAsync(int idTrip, string pesel)
    {
        return await tripDbContext.ClientTrips
            .Include(ct => ct.IdClientNavigation)
            .AnyAsync(ct => ct.IdTrip == idTrip && ct.IdClientNavigation.Pesel == pesel);
    }

    public async Task AddClientAsync(Core.Models.Client client)
    {
        await tripDbContext.Clients.AddAsync(client);
    }

    public async Task AddClientTripAsync(Core.Models.ClientTrip clientTrip)
    {
        await tripDbContext.ClientTrips.AddAsync(clientTrip);
    }

    public async Task SaveChangesAsync()
    {
        await tripDbContext.SaveChangesAsync();
    }
}
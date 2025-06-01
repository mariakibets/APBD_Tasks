using TripApp.Application.DTOs;
using TripApp.Application.Mappers;
using TripApp.Application.Repository;
using TripApp.Application.Services.Interfaces;
using TripApp.Core.Models;

namespace TripApp.Application.Services;

public class TripService(ITripRepository tripRepository) : ITripService
{
    public async Task<PaginatedResult<GetTripDto>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 10) pageSize = 10;
        var result = await tripRepository.GetPaginatedTripsAsync(page, pageSize);

        var mappedTrips = new PaginatedResult<GetTripDto>
        {
            AllPages = result.AllPages,
            Data = result.Data.Select(trip => trip.MapToGetTripDto()).ToList(),
            PageNum = result.PageNum,
            PageSize = result.PageSize
        };

        return mappedTrips;
    }

    public async Task<List<GetTripDto>> GetAllTripsAsync()
    {
        var trips = await tripRepository.GetAllTripsAsync();
        var mappedTrips = trips.Select(trip => trip.MapToGetTripDto()).ToList();
        return mappedTrips;
    }
    
    
    public async Task AssignClientToTripAsync(int idTrip, AssignClientDTO request)
    {
        var trip = await tripRepository.GetTripByIdAsync(idTrip);
        if (trip == null)
            throw new Exception("Trip not found.");

        if (trip.DateFrom <= DateTime.UtcNow)
            throw new Exception("Trip has already started.");

        if (await tripRepository.ClientExistsByPeselAsync(request.Pesel) &&
            await tripRepository.IsClientRegisteredToTripAsync(idTrip, request.Pesel))
        {
            throw new Exception("Client with this PESEL is already registered for the trip.");
        }

        var client = new Client
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Telephone = request.PhoneNumber,
            Pesel = request.Pesel
        };

        await tripRepository.AddClientAsync(client);
        await tripRepository.SaveChangesAsync();

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.UtcNow,
            PaymentDate = request.PaymentDate
        };

        await tripRepository.AddClientTripAsync(clientTrip);
        await tripRepository.SaveChangesAsync();
    }
}
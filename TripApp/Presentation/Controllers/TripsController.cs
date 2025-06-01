using Microsoft.AspNetCore.Mvc;
using TripApp.Application.DTOs;
using TripApp.Application.Exceptions;
using TripApp.Application.Services.Interfaces;
using TripApp.Core.Models;

namespace TripApp.Presentation.Controllers;

[ApiController]
[Route("api/trips")]
public class TripController(
    ITripService tripService) 
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<GetTripDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<GetTripDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrips(
        [FromQuery(Name = "page")] int? page,
        [FromQuery(Name = "pageSize")] int? pageSize,
        CancellationToken cancellationToken = default)
    {
        if (page is null && pageSize is null)
        {
            var trips = await tripService.GetAllTripsAsync();
            return Ok(trips);
        }

        var paginatedTrips = await tripService.GetPaginatedTripsAsync(page ?? 1, pageSize ?? 10);
        return Ok(paginatedTrips);
    }
    
    
     
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] AssignClientDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await tripService.AssignClientToTripAsync(idTrip, request);
            return Ok(new { message = "Client successfully assigned to the trip." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
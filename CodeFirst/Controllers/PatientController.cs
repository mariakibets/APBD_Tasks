using CodeFirst.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers;

[ApiController]
[Route("api/patient")]
public class PatientController : ControllerBase
{
    private IPatientService _service;

    public PatientController(IPatientService service)
    {
        _service = service;
    }
    
    [HttpGet("patient/{id}")]
    public async Task<IActionResult> GetPatientDetails(int id)
    {
        var patientDto = await _service.GetPatientDetailsAsync(id);
        if (patientDto == null)
            return NotFound($"Patient with ID {id} not found.");

        return Ok(patientDto);
    }

}
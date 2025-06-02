using CodeFirst.DTO;
using CodeFirst.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> IssuePrescription([FromBody] PerscriptionDTO request)
    {
        var result = await _service.IssuePrescriptionAsync(request);

        if (result.Contains("does not exist") || result.Contains("cannot"))
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    
    
}
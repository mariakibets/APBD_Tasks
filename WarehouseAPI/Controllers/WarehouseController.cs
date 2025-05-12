using Microsoft.AspNetCore.Mvc;
using WarehouseAPI.Contracts.Requests;
using WarehouseAPI.Repositories;

namespace WarehouseAPI.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseRepository _warehouseRepository;
    
    public WarehouseController(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }
    
    [HttpGet("product/{productId}")]
    public async Task<IActionResult> ProductExists(int productId, CancellationToken cancellationToken)
    {
        var exists = await _warehouseRepository.ProductExistsAsync(productId, cancellationToken);
        if (!exists)
        {
            return NotFound(new { Message = "Product not found." });
        }

        return Ok(new { Message = "Product exists." });
    }
    
    [HttpGet("warehouse/{warehouseId}")]
    public async Task<IActionResult> WarehouseExists(int warehouseId, CancellationToken cancellationToken)
    {
        var exists = await _warehouseRepository.WarehouseExistsAsync(warehouseId, cancellationToken);
        if (!exists)
        {
            return NotFound(new { Message = "Warehouse not found." });
        }

        return Ok(new { Message = "Warehouse exists." });
    }

   
    [HttpGet("product-order")]
    public async Task<IActionResult> ProductInOrder(int productId, int amount, DateTime createdAt,
        CancellationToken cancellationToken)
    {
        var isInOrder =
            await _warehouseRepository.ProductInTheOrderTable(productId, amount, createdAt, cancellationToken);
        if (!isInOrder)
        {
            return NotFound(new { Message = "Product not in the order table." });
        }

        return Ok(new { Message = "Product exists in the order table." });
    }
    
    
    [HttpPost("add-product")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddProduct([FromBody] WarehouseRequest request,
        CancellationToken cancellationToken)
    {
       
        if (request.Amount <= 0)
        {
            return BadRequest(new { Message = "Amount should be greater than zero." });
        }
        
        var productId = await _warehouseRepository.AddProductAsync(request, cancellationToken);
        if (productId == null)
        {
            return BadRequest(new { Message = "Failed to add product." });
        }

        return Ok(new { Message = "Product added successfully." });
    }
    
    [HttpPost("add-product-sp")]
    public async Task<IActionResult> AddProductUsingStoredProcedure([FromBody] WarehouseRequest request)
    {
        var result = await _warehouseRepository.AddProductUsingStoredProcedureAsync(request);
        if (result == null)
        {
            return BadRequest(new { Message = "Failed to add product using stored procedure." });
        }

        return Ok(new { Message = "Product added using stored procedure.", ProductWarehouseId = result });
    }
}
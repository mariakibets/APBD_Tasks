using WarehouseAPI.Contracts.Requests;

namespace WarehouseAPI.Repositories;

public interface IWarehouseRepository
{
    Task<bool> ProductExistsAsync(int productId, CancellationToken cancellationToken = default);
    Task<bool> WarehouseExistsAsync(int warehouseId, CancellationToken cancellationToken = default);
    Task<bool> ProductInTheOrderTable(int productId, int amount, DateTime createdAt, CancellationToken cancellationToken = default);
    Task<int?> AddProductAsync(WarehouseRequest request, CancellationToken cancellationToken = default);
    Task<int?> AddProductUsingStoredProcedureAsync(WarehouseRequest request);

}
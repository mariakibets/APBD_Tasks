using Microsoft.Data.SqlClient;
using WarehouseAPI.Contracts.Requests;

namespace WarehouseAPI.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly string _connectionString;
    
    public WarehouseRepository(IConfiguration cfg)
    {
        _connectionString = cfg.GetConnectionString("Default") ??
                            throw new ArgumentNullException(nameof(cfg),
                                "Default connection string is missing in configuration");
    }

    public async Task<bool> ProductExistsAsync(int productId, CancellationToken cancellationToken = default)
    {
        const string query = """
                             SELECT 
                                 IIF(EXISTS (SELECT 1 FROM Product 
                                         WHERE Product.IdProduct = @productId), 1, 0);   
                             """;
        await using SqlConnection con = new(_connectionString);
        await using SqlCommand command = new SqlCommand(query, con);
        await con.OpenAsync(cancellationToken);
        command.Parameters.AddWithValue("@productId", productId);
        var result = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));

        return result == 1;
    }

    public async Task<bool> WarehouseExistsAsync(int warehouseId, CancellationToken cancellationToken = default)
    {
        const string query = """
                             SELECT IIF(EXISTS (SELECT 1 FROM Warehouse
                                WHERE Warehouse.IdWarehouse = @warehouseId), 1, 0); 
                             """;
        await using SqlConnection con = new(_connectionString);
        await using SqlCommand command = new SqlCommand(query, con);
        await con.OpenAsync(cancellationToken);
        command.Parameters.AddWithValue("@warehouseId", warehouseId );
        var result = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));

        return result == 1;                                    
    }

    public async Task<bool> ProductInTheOrderTable(int productId, int amount, DateTime createdAt, CancellationToken cancellationToken = default)
    {
        const string query = """
                             SELECT TOP 1 o.IdOrder
                             FROM Orders o
                             LEFT JOIN Product_Warehouse pw ON o.IdOrder = pw.IdOrder
                             WHERE o.IdProduct = @productId
                               AND o.Amount = @amount
                               AND o.CreatedAt < @createdAt
                               AND pw.IdOrder IS NULL
                             """;
    
        await using SqlConnection con = new(_connectionString);
        await using SqlCommand command = new SqlCommand(query, con);
        command.Parameters.AddWithValue("@productId", productId);  
        command.Parameters.AddWithValue("@amount", amount);        
        command.Parameters.AddWithValue("@createdAt", createdAt);  

        await con.OpenAsync(cancellationToken);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        
        return result != null;
    }


    public async Task<int?> AddProductAsync(WarehouseRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Amount <= 0) return -1;

        await using SqlConnection con = new(_connectionString);
        await con.OpenAsync(cancellationToken);
        await using var transaction = await con.BeginTransactionAsync(cancellationToken);

        try
        {
            if (!await ProductExistsAsync(request.ProductId, cancellationToken) ||
                !await WarehouseExistsAsync(request.WarehouseId, cancellationToken))
            {
                await transaction.RollbackAsync(cancellationToken);
                return -1;
            }
            
            const string orderQuery = """
                                      SELECT TOP 1 o.IdOrder
                                      FROM Orders o
                                      LEFT JOIN Product_Warehouse pw ON o.IdOrder = pw.IdOrder
                                      WHERE o.IdProduct = @productId
                                        AND o.Amount = @amount
                                        AND o.CreatedAt < @createdAt
                                        AND pw.IdOrder IS NULL;
                                      """;

            await using var getOrderCmd = new SqlCommand(orderQuery, con, (SqlTransaction)transaction);
            getOrderCmd.Parameters.AddWithValue("@productId", request.ProductId);
            getOrderCmd.Parameters.AddWithValue("@amount", request.Amount);
            getOrderCmd.Parameters.AddWithValue("@createdAt", request.CreatedAt);

            var orderIdObj = await getOrderCmd.ExecuteScalarAsync(cancellationToken);
            if (orderIdObj == null)
            {
                await transaction.RollbackAsync(cancellationToken);
                return -1;
            }

            var orderId = Convert.ToInt32(orderIdObj);

            const string getPriceQuery = "SELECT Price FROM Product WHERE IdProduct = @productId";
            await using var getPriceCmd = new SqlCommand(getPriceQuery, con, (SqlTransaction)transaction);
            getPriceCmd.Parameters.AddWithValue("@productId", request.ProductId);
            var priceObj = await getPriceCmd.ExecuteScalarAsync(cancellationToken);
            if (priceObj == null)
            {
                await transaction.RollbackAsync(cancellationToken);
                return -1;
            }

            decimal unitPrice = Convert.ToDecimal(priceObj);
            decimal totalPrice = unitPrice * request.Amount;
            
            const string updateOrderQuery = """
                                            UPDATE Orders
                                            SET FulfilledAt = @fulfilledAt
                                            WHERE IdOrder = @orderId;
                                            """;

            await using var updateCmd = new SqlCommand(updateOrderQuery, con, (SqlTransaction)transaction);
            updateCmd.Parameters.AddWithValue("@fulfilledAt", DateTime.Now);
            updateCmd.Parameters.AddWithValue("@orderId", orderId);
            await updateCmd.ExecuteNonQueryAsync(cancellationToken);
            
            const string insertQuery = """
                                       INSERT INTO Product_Warehouse (ProductId, WarehouseId, IdOrder, Amount, Price, CreatedAt)
                                       OUTPUT INSERTED.IdProductWarehouse
                                       VALUES (@productId, @warehouseId, @orderId, @amount, @price, @createdAt);
                                       """;

            await using var insertCmd = new SqlCommand(insertQuery, con, (SqlTransaction)transaction);
            insertCmd.Parameters.AddWithValue("@productId", request.ProductId);
            insertCmd.Parameters.AddWithValue("@warehouseId", request.WarehouseId);
            insertCmd.Parameters.AddWithValue("@orderId", orderId);
            insertCmd.Parameters.AddWithValue("@amount", request.Amount);
            insertCmd.Parameters.AddWithValue("@price", totalPrice);
            insertCmd.Parameters.AddWithValue("@createdAt", DateTime.Now);

            var insertResult = await insertCmd.ExecuteScalarAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return insertResult != null ? Convert.ToInt32(insertResult) : (int?)null;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<int?> AddProductUsingStoredProcedureAsync(WarehouseRequest request)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("AddProductToWarehouse", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@IdProduct", request.ProductId);
        cmd.Parameters.AddWithValue("@IdWarehouse", request.WarehouseId);
        cmd.Parameters.AddWithValue("@Amount", request.Amount);
        cmd.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);

        await conn.OpenAsync();

        try
        {
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? (int?)result : null;
        }
        catch (SqlException ex) when (ex.Message.Contains("There is no order to fullfill"))
        {
            throw new InvalidOperationException("The stored procedure reported: no matching unfulfilled order.");
        }
    }
}
using Dapper;
using Server.Models;

namespace Server.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public OrderRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<int> CreateOrderAsync(Order newOrder, IEnumerable<Component> components)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();
        try
        {
            var buildSql = "INSERT INTO ComputerBuilds (Name, Category) VALUES (@Name, @Category); SELECT CAST(SCOPE_IDENTITY() as int);";
            var buildId = await connection.QuerySingleAsync<int>(buildSql, new { Name = $"Custom Build for Order", Category = "Custom" }, transaction);
            newOrder.BuildId = buildId;

            var buildComponentsSql = "INSERT INTO BuildComponents (build_id, component_id, quantity) VALUES (@BuildId, @ComponentId, @Quantity);";
            foreach (var component in components)
            {
                await connection.ExecuteAsync(buildComponentsSql, new { BuildId = buildId, ComponentId = component.Id, Quantity = 1 }, transaction);
            }

            var orderSql = "INSERT INTO Orders (user_id, build_id, total_price, status, order_date) VALUES (@UserId, @BuildId, @TotalPrice, @Status, @OrderDate); SELECT CAST(SCOPE_IDENTITY() as int);";
            var orderId = await connection.QuerySingleAsync<int>(orderSql, newOrder, transaction);

            transaction.Commit();
            return orderId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var sql = """
                  SELECT 
                      id, 
                      user_id AS UserId, 
                      build_id AS BuildId, 
                      total_price AS TotalPrice, 
                      status, 
                      order_date AS OrderDate 
                  FROM Orders 
                  WHERE user_id = @UserId 
                  ORDER BY order_date DESC;
                  """;

        return await connection.QueryAsync<Order>(sql, new { UserId = userId });
    }
}
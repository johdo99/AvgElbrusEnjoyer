using Dapper;
using Server.Models;

namespace Server.Data.Repositories;

public class ComponentRepository : IComponentRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public ComponentRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<int> AddAsync(Component component)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = """
                  INSERT INTO Components (Name, Type, Price, Stock)
                  VALUES (@Name, @Type, @Price, @Stock);
                  SELECT CAST(SCOPE_IDENTITY() as int);
                  """;
        return await connection.QuerySingleAsync<int>(sql, component);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = "DELETE FROM Components WHERE Id = @Id";
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }

    public async Task<IEnumerable<Component>> GetAllAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = "SELECT * FROM Components";
        return await connection.QueryAsync<Component>(sql);
    }

    public async Task<IEnumerable<Component>> GetByFilterAsync(string nameFilter)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = "SELECT * FROM Components WHERE Name LIKE @Pattern";
        return await connection.QueryAsync<Component>(sql, new { Pattern = $"%{nameFilter}%" });
    }

    public async Task<Component?> GetByIdAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = "SELECT * FROM Components WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<Component>(sql, new { Id = id });
    }

    public async Task<bool> UpdateAsync(Component component)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var sql = """
                  UPDATE Components 
                  SET Name = @Name, 
                      Type = @Type, 
                      Price = @Price, 
                      Stock = @Stock
                  WHERE Id = @Id
                  """;
        var affectedRows = await connection.ExecuteAsync(sql, component);
        return affectedRows > 0;
    }
}
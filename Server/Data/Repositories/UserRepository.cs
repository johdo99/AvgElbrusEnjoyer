using Dapper;
using Server.Models;

namespace Server.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public UserRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<int> AddAsync(User user)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var sql = """
                  INSERT INTO Users (Username, PasswordHash, Role)
                  VALUES (@Username, @PasswordHash, @Role);
                  SELECT CAST(SCOPE_IDENTITY() as int);
                  """;

        // Dapper выполняет запрос и передает параметры из объекта user
        var newId = await connection.QuerySingleAsync<int>(sql, user);
        return newId;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var sql = "SELECT * FROM Users WHERE Username = @Username";

        // Dapper выполняет запрос и автоматически создает объект User из результата
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
    }
}
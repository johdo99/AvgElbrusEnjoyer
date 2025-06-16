using Microsoft.Data.SqlClient;
using System.Data;

namespace Server.Data;

/// <summary>
/// Фабрика для создания подключений к базе данных
/// </summary>
public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory()
    {
        _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=AvgElbrusEnjoyerDb;Trusted_Connection=True;";
    }

    /// <summary>
    /// Создает и возвращает новое подключение к БД
    /// </summary>
    /// <returns>Объект подключения, реализующий IDbConnection</returns>
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
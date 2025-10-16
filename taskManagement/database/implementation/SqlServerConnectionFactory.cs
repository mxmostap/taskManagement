using System.Data;
using Microsoft.Data.SqlClient;

namespace taskManagement.database.implementation;

public class SqlServerConnectionFactory: IDatabaseConnectionFactory
{
    private readonly string _connectionString;

    public SqlServerConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
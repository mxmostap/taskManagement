using System.Data;
using Dapper;

namespace taskManagement.database.impl;

public class DatabaseInitializer: IDatabaseInitializer
{
    private readonly IDatabaseConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = _connectionFactory.GetConnection();
            var result = await connection.ExecuteScalarAsync<string>("SELECT DB_NAME()");
            return !string.IsNullOrEmpty(result);
        }
        catch
        {
            return false;
        }
    }

    public async Task InitializeAsync()
    {
        using var connection = _connectionFactory.GetConnection();
        await CreateDatabaseIfNotExistsAsync(connection);
        await SwitchToApplicationDatabaseAsync(connection);
        await CreateTableAsync(connection);
        await AddTestData(connection);
    }

    private async Task CreateDatabaseIfNotExistsAsync(IDbConnection connection)
    {
        const string dbName = "taskManagement";
        var sqlDbCreate = $"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{dbName}')" +
                          $" BEGIN CREATE DATABASE [{dbName}]; " +
                          $" PRINT 'Database {dbName} created.';END";

        await connection.ExecuteAsync(sqlDbCreate);
    }

    private async Task SwitchToApplicationDatabaseAsync(IDbConnection connection)
    {
        var currentConnectionString = connection.ConnectionString;
        if (!currentConnectionString.Contains("Database=taskManagement"))
        {
            connection.ConnectionString = currentConnectionString
                .Replace("Database=master", "Database=taskManagement");
            
            if (connection.State == ConnectionState.Open)
                connection.Close();
            
            connection.Open();
        }
    }

    private async Task CreateTableAsync(IDbConnection connection)
    {
        const string createTasksTable = "IF NOT EXISTS (SELECT * FROM sysobjects " + 
                                        "WHERE name='Tasks' AND xtype='U') " +
                                        "BEGIN CREATE TABLE Tasks (" +
                                        "Id int identity constraint Tasks_pk primary key, " +
                                        "Title nvarchar(30) not null, Description nvarchar(60), " +
                                        "IsCompleted bit, CreatedAt datetime not null);END";
        await connection.ExecuteAsync(createTasksTable);
    }

    private async Task AddTestData(IDbConnection connection)
    {
        var tasksDbNumber = await connection.ExecuteScalarAsync<int>
            ("SELECT COUNT(*) FROM Tasks");

        if (tasksDbNumber == 0)
        {
            await connection.ExecuteAsync("INSERT INTO " +
                    "Tasks (Title, Description, IsCompleted, CreatedAt) " +
                    "VALUES ('TestTitle', 'TestDescription', 0, GETUTCDATE())");
        }
    }
}
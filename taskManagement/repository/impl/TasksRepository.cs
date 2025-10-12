using Dapper;
using taskManagement.database;
using taskManagement.entity;

namespace taskManagement.repository.impl;

public class TasksRepository: ITasksRepository
{
    private readonly IDatabaseConnectionFactory _connectionFactory;
    
    public TasksRepository(IDatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Tasks?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.GetConnection();
        const string sqlQuery = "SELECT * FROM Tasks WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Tasks>(sqlQuery, new {ID = id});
    }
    
    public async Task<int> CreateAsync(Tasks task)
    {
        using var connection = _connectionFactory.GetConnection();
        const string sqlQuery = "INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt) " +
                                "VALUES (@Title, @Description, @IsCompleted, @CreatedAt);" +
                                "SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sqlQuery, task);
    }

    public async Task<IEnumerable<Tasks>> GetTasksAsync()
    {
        using var connection = _connectionFactory.GetConnection();
        const string sqlQuery = "SELECT * FROM Tasks";
        return await connection.QueryAsync<Tasks>(sqlQuery);
    }

    public async Task<bool> UpdateStatusAsync(int id, bool status)
    {
        using var connection = _connectionFactory.GetConnection();
        const string sqlQuery = "UPDATE Tasks SET IsCompleted = @IsCompleted WHERE Id = @Id";
        var updatedRows = await connection.ExecuteAsync(sqlQuery, 
            new {Id = id, IsCompleted = status});
        return updatedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _connectionFactory.GetConnection();
        const string sqlQuery = "DELETE FROM Tasks where Id = @Id;";
        var deletedRows = await connection.ExecuteAsync(sqlQuery, new {Id = id});
        return deletedRows > 0;
    }
}
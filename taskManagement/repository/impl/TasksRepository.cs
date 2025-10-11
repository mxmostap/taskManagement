using System.Data;
using Dapper;
using taskManagement.Entities.Interfaces;

namespace taskManagement.Entities;

public class TasksRepository: ITasksRepository
{
    private readonly IDbConnection _connectionString;

    public TasksRepository(IDbConnection connectionString)
    {
        this._connectionString = connectionString;
    }

    public async Task<int> CreateAsync(Tasks tasks)
    {
        const string sqlQuery = "INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt) " +
                                "VALUES (@Title, @Description, @IsCompleted, NOW());" +
                                "SELECT CAST(SCOPE_IDENTITY() as int)";
        return await _connectionString.ExecuteScalarAsync<int>(sqlQuery, tasks);
    }

    public async Task<IEnumerable<Tasks>> GetTasksAsync()
    {
        const string sqlQuery = "SELECT * FROM Tasks";
        return await _connectionString.QueryAsync<Tasks>(sqlQuery);
    }

    public async Task<bool> UpdateStatusAsync(int id, bool status)
    {
        const string sqlQuery = "UPDATE Tasks SET IsCompleted = @IsCompleted WHERE Id = @Id";
        var updatedRows = await _connectionString.ExecuteAsync(sqlQuery, 
            new {Id = id, IsCompleted = status});
        return updatedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sqlQuery = "DELETE FROM Tasks where Id = @Id;";
        var deletedRows = await _connectionString.ExecuteAsync(sqlQuery, new {Id = id});
        return deletedRows > 0;
    }
}
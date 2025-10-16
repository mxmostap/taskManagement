using taskManagement.entities;

namespace taskManagement.services;

public interface ITasksService
{
    Task<Tasks?> GetTaskAsync(int id);
    Task<Tasks> CreateTaskAsync(Tasks task);
    Task<IEnumerable<Tasks>> GetTasksAsync();
    Task<bool> UpdateTaskAsync(Tasks task);
    Task<bool> DeleteTaskAsync(int id);
}
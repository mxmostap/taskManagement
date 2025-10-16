using taskManagement.entity;

namespace taskManagement.service;

public interface ITasksService
{
    Task<Tasks?> GetTaskAsync(int id);
    Task<Tasks> CreateTaskAsync(Tasks task);
    Task<IEnumerable<Tasks>> GetTasksAsync();
    Task<Tasks> UpdateTaskStatusAsync(int id, bool status);
    Task<bool> DeleteTaskAsync(int id);
}
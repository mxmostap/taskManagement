using taskManagement.entity;

namespace taskManagement.repository;

public interface ITasksRepository
{
    Task<Tasks?> GetByIdAsync(int id);
    Task<int> CreateAsync(Tasks task);
    Task<IEnumerable<Tasks>> GetTasksAsync();
    Task<bool> UpdateStatusAsync(int id, bool status);
    Task<bool> DeleteAsync(int id);
}
using taskManagement.entities;

namespace taskManagement.repositories;

public interface ITasksRepository
{
    Task<Tasks?> GetByIdAsync(int id);
    Task<int> CreateAsync(Tasks task);
    Task<IEnumerable<Tasks>> GetTasksAsync();
    Task<bool> UpdateAsync(Tasks task);
    Task<bool> DeleteAsync(int id);
}
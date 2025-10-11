namespace taskManagement.Entities.Interfaces;

public interface ITasksRepository
{
    Task<int> CreateAsync(Tasks tasks);
    Task<IEnumerable<Tasks>> GetTasksAsync();
    Task<bool> UpdateStatusAsync(int id, bool status);
    Task<bool> DeleteAsync(int id);
}
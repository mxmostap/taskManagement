using taskManagement.entities;
using taskManagement.repositories;

namespace taskManagement.services.implementation;

public class TasksService: ITasksService
{
    private ITasksRepository _tasksRepository;

    public TasksService(ITasksRepository tasksRepository)
    {
        _tasksRepository = tasksRepository;
    }
    
    public async Task<Tasks?> GetTaskAsync(int id)
    {
        var task = await _tasksRepository.GetByIdAsync(id);
        if (task == null) throw new Exception("Задачи с таким Id не существует!");
        else return task;
    }

    public async Task<Tasks> CreateTaskAsync(Tasks task)
    {
        task.Id = await _tasksRepository.CreateAsync(task);
        return task;
    }

    public async Task<IEnumerable<Tasks>> GetTasksAsync()
    {
        return await _tasksRepository.GetTasksAsync();
    }

    public async Task<bool> UpdateTaskAsync(Tasks task)
    {
        await GetTaskAsync(task.Id);
        return await _tasksRepository.UpdateAsync(task);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await _tasksRepository.DeleteAsync(id);
    }
}
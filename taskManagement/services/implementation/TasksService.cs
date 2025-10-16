using taskManagement.entity;
using taskManagement.repository;

namespace taskManagement.service.impl;

public class TasksService: ITasksService
{
    private ITasksRepository _tasksRepository;

    public TasksService(ITasksRepository tasksRepository)
    {
        _tasksRepository = tasksRepository;
    }
    
    public async Task<Tasks?> GetTaskAsync(int id)
    {
        return await _tasksRepository.GetByIdAsync(id);
    }

    public async Task<Tasks> CreateTaskAsync(Tasks task)
    {
        //var id = 
        task.Id = await _tasksRepository.CreateAsync(task);
        return task;
    }

    public async Task<IEnumerable<Tasks>> GetTasksAsync()
    {
        return await _tasksRepository.GetTasksAsync();
    }

    public async Task<Tasks> UpdateTaskStatusAsync(int id, bool status)
    {
        await _tasksRepository.UpdateStatusAsync(id, status);
        return await _tasksRepository.GetByIdAsync(id);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await _tasksRepository.DeleteAsync(id);
    }
}
namespace taskManagement.UI;

public interface IConsoleMenu
{
    Task ShowMainMenuAsync();
    Task CreateTaskAsync();
    Task ShowAllTasksAsync();
    Task UpdateTaskStatusAsync();
    Task DeleteTaskAsync();
}
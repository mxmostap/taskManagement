using taskManagement.entities;
using taskManagement.services;
using taskManagement.UI;
using taskManagement.ui.helpers;

namespace taskManagement.ui.implementation;

public class ConsoleMenu : IConsoleMenu
{
    private readonly ITasksService _tasksService;

    public ConsoleMenu(ITasksService tasksService)
    {
        _tasksService = tasksService;
    }

    public async Task ShowMainMenuAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Приложение управления задачами");
            ShowMainMenuFunctions();
            var choice = InputValidator.GetNumberBetween(0, 4);
            await ProcessMaimMenuChoiceAsync(choice);
            if (choice == 0) break;

            Console.WriteLine("Для продолжения работы нажмите любую клавишу");
            Console.ReadKey();
        }
    }

    public async Task CreateTaskAsync()
    {
        Console.WriteLine("Создание новой задачи:");
        var title = InputValidator.GetString("название");
        var description = InputValidator.GetString("описание задачи", true);
        var isCompleted = InputTaskInfo.GetTaskStatus();
        var createdAt = InputTaskInfo.GetCreatedAtDataTime();
        var task = new Tasks
        {
            Title = title,
            Description = description,
            IsCompleted = isCompleted,
            CreatedAt = createdAt
        };
        var createdTask = await _tasksService.CreateTaskAsync(task);
    }

    public async Task ShowAllTasksAsync()
    {
        var tasks = await _tasksService.GetTasksAsync();

        TaskDisplayer.DisplayAllTasks(tasks);
    }

    public async Task UpdateTaskStatusAsync()
    {
        try
        {
            var taskId = InputValidator.GetNumber("Введите Id задачи," +
                                                  "у которой хотите обновить статус: ");
            var task = await _tasksService.GetTaskAsync(taskId);

            Console.WriteLine($"Текущий статус задачи : " +
                              $"{(task.IsCompleted ? "выполнена" : "не выполнена")}");
            task.IsCompleted = InputTaskInfo.GetTaskStatus();
            var result = await _tasksService.UpdateTaskAsync(task);
            if (result)
            {
                Console.WriteLine("Обновленная задача:");
                TaskDisplayer.Display(task);
            }
            else
                Console.WriteLine("ОШИБКА: Статус задачи не обновлен!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }

    public async Task DeleteTaskAsync()
    {
        try
        {
            await ShowAllTasksAsync();
            var taskId = InputValidator.GetNumber("Введите Id задачи," +
                                                  "которую хотите удалить: ");
            var task = await _tasksService.GetTaskAsync(taskId);
            
            Console.WriteLine("Задача, которую Вы хотите удалить:");
            TaskDisplayer.Display(task);
            Console.WriteLine("Вы уверены что хотите удалить данную задачу?\n" +
                              "1 - Да\n2 - Нет");
            var deleteStatus = InputValidator.GetNumberBetween(1, 2);
            if (deleteStatus == 1)
            {
                var result = await _tasksService.DeleteTaskAsync(taskId);
                if (result)
                    Console.WriteLine("Задача удалена");
                else
                    Console.WriteLine("Ошибка удаления задачи");
            }
            else
                Console.WriteLine("Вы отменили удаление задачи");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }

    private async Task ProcessMaimMenuChoiceAsync(int choice)
    {
        switch (choice)
        {
            case 1:
                await CreateTaskAsync();
                break;
            case 2:
                await ShowAllTasksAsync();
                break;
            case 3:
                await UpdateTaskStatusAsync();
                break;
            case 4:
                await DeleteTaskAsync();
                break;
            case 0:
                Console.WriteLine("Выход из приложения. Спасибо за работу!");
                break;
            default:
                Console.WriteLine("Введено недопустимое значение!");
                break;
        }
    }

    private void ShowMainMenuFunctions()
    {
        Console.WriteLine("Главное меню:");
        Console.WriteLine("1 - Добавить новую задачу");
        Console.WriteLine("2 - Просмотр всех задач");
        Console.WriteLine("3 - Обновление статуса задачи");
        Console.WriteLine("4 - Удаление задачи");
        Console.WriteLine("0 - Выход");
        Console.Write("Введите номер пункта меню: ");
    }
}
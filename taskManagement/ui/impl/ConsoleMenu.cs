using taskManagement.entity;
using taskManagement.service;
using taskManagement.ui.helpers;

namespace taskManagement.UI.impl;

public class ConsoleMenu: IConsoleMenu
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
        var taskId = InputValidator.GetNumber("Введите Id задачи," +
                                              "у которой хотите обновить статус: ");
        var task = _tasksService.GetTaskAsync(taskId);
        if (task == null)
        {
            Console.WriteLine("Задача с таким Id не найдена");
            return;
        }

        Console.WriteLine($"Текущий статус задачи : " +
                          $"{(task.IsCompleted ? "выполнена" : "не выполнена")}");
        var newTaskStatus = InputTaskInfo.GetTaskStatus();
        var result = _tasksService.UpdateTaskStatusAsync(taskId, newTaskStatus);
        Console.WriteLine("Статус задачи обновлен");
    }

    public async Task DeleteTaskAsync()
    {
        await ShowAllTasksAsync();
        var taskId = InputValidator.GetNumber("Введите Id задачи," +
                                              "которую хотите удалить: ");
        var task = await _tasksService.GetTaskAsync(taskId);
        if (task == null)
        {
            Console.WriteLine("Задача с таким Id не найдена");
            return;
        }

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
        Console.WriteLine("3 - Обновление состояния задачи");
        Console.WriteLine("4 - Удаление задачи");
        Console.WriteLine("0 - Выход");
        Console.Write("Введите номер пункта меню: ");
    }
}
using taskManagement.entity;
using taskManagement.service;

namespace taskManagement.UI.impl;

public class ConsoleMenu: IConsoleMenu
{
    private readonly ITasksService _tasksService;
    private readonly InputValidator _inputValidator;

    public ConsoleMenu(ITasksService tasksService)
    {
        _tasksService = tasksService;
        _inputValidator = new InputValidator();
    }

    public async Task ShowMainMenuAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Приложение управления задачами");
            ShowMainMenuFunctions();
            var choice = _inputValidator.GetInt(0, 4);
            await ProcessMaimMenuChoiceAsync(choice);
            if (choice == 0) break;

            Console.WriteLine("Для продолжения работы нажмите любую клавишу");
            Console.ReadKey();
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
                Console.WriteLine("3 - Обновление состояния задачи");
                Console.WriteLine("4 - Удаление задачи");
                Console.WriteLine("0 - Выход");
                Console.Write("Введите номер пункта меню: ");
    }

    public async Task CreateTaskAsync()
    {
        Console.WriteLine("Создание новой задачи:");
        var title = _inputValidator.GetRequiredString("название", 
            "Название не может быть пустым!");
        var description = _inputValidator.GetOptionalString("описание задачи");
        var isCompleted = _inputValidator.GetTaskStatus();
        var createdAt = _inputValidator.GetCreatedAt();
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

        Console.WriteLine("Список всех задач:");
        if (!tasks.Any())
        {
            Console.WriteLine("Задачи не найдены!");
            return;
        }
        DisplayAllTasks(tasks);
    }

    public async Task UpdateTaskStatusAsync()
    {
        var taskId = _inputValidator.GetTaskId();
        var task = _tasksService.GetTaskAsync(taskId);
        if (task == null)
        {
            Console.WriteLine("Задача с таким Id не найдена");
            return;
        }

        Console.WriteLine($"Текущий статус задачи : " +
                          $"{(task.IsCompleted ? "выполнена" : "не выполнена")}");
        var newTaskStatus = _inputValidator.GetTaskStatus();
        var result = _tasksService.UpdateTaskStatusAsync(taskId, newTaskStatus);
        Console.WriteLine("Статус задачи обновлен");
    }

    public async Task DeleteTaskAsync()
    {
        await ShowAllTasksAsync();
        var taskId = _inputValidator.GetTaskId();
        var task = await _tasksService.GetTaskAsync(taskId);
        if (task == null)
        {
            Console.WriteLine("Задача с таким Id не найдена");
            return;
        }

        Console.WriteLine("Задача, которую Вы хотите удалить:");
        DisplayTask(task);
        Console.WriteLine("Вы уверены что хотите удалить данную задачу?\n" +
                          "1 - Да\n2 - Нет");
        var deleteStatus = _inputValidator.GetInt(1, 2);
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

    private void DisplayAllTasks(IEnumerable<Tasks> tasks)
    {
        foreach (var task in tasks)
        {
            DisplayTask(task);
        }
    }

    private void DisplayTask(Tasks task)
    {
        Console.WriteLine($"ID: {task.Id}");
        Console.WriteLine($"Название: {task.Title}");
        Console.WriteLine($"Описание: {task.Description}");
        Console.WriteLine($"Статус: {(task.IsCompleted ? "выполнена" : "не выполнена")}");
        Console.WriteLine($"Дата создания: {task.CreatedAt}");
    }
}
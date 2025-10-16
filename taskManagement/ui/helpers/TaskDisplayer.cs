using taskManagement.entities;

namespace taskManagement.ui.helpers;

public class TaskDisplayer
{
    public static void DisplayAllTasks(IEnumerable<Tasks> tasks)
    {
        if (!tasks.Any())
        {
            Console.WriteLine("Задачи не найдены!");
            return;
        }
        Console.WriteLine("Список всех задач:");
        foreach (var task in tasks)
        {
            Display(task);
            Console.WriteLine(new string('-', 50));
        }
    }

    public static void Display(Tasks task)
    {
        Console.WriteLine($"ID: {task.Id}");
        Console.WriteLine($"Название: {task.Title}");
        Console.WriteLine($"Описание: {task.Description}");
        Console.WriteLine($"Статус: {(task.IsCompleted ? "выполнена" : "не выполнена")}");
        Console.WriteLine($"Дата создания: {task.CreatedAt}");
    }
}
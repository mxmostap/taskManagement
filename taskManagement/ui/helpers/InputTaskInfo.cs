namespace taskManagement.ui.helpers;

public class InputTaskInfo
{
    public static DateTime GetCreatedAtDataTime()
    {
        Console.WriteLine("Дата и время создания задачи:");
        Console.WriteLine("1 - Текущая дата и время\n2 - Ввести в ручную");
        if (InputValidator.GetNumberBetween(1, 2) == 1)
            return DateTime.Now;
        else
            return InputValidator.GetInputDataTime();
    }
    
    public static bool GetTaskStatus()
    {
        Console.WriteLine("Выберите статус задачи:");
        Console.WriteLine("1 - Выполнена\n2 - Не выполнена");
        var inputStatus = InputValidator.GetNumberBetween(1, 2);
        if (inputStatus == 1) return true;
        else return false;
    }
}
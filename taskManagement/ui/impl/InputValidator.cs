using Microsoft.IdentityModel.Tokens;

namespace taskManagement.UI.impl;

public class InputValidator
{
    public string GetRequiredString(string inputDescription, string ruleMessage)
    {
        while (true)
        {
            Console.WriteLine($"Введите {inputDescription}");
            string? input = Console.ReadLine();
            if (!input.IsNullOrEmpty())
                return input.Trim();
            else
                Console.WriteLine(ruleMessage);
        }
    }

    public string? GetOptionalString(string inputDescription)
    {
        Console.WriteLine($"Введите {inputDescription}");
        string? input = Console.ReadLine();
        return input.IsNullOrEmpty() ? null : input.Trim();
    }

    public bool GetTaskStatus()
    {
        Console.WriteLine("Выберите статус задачи:");
        Console.WriteLine("1 - Выполнена\n2 - Не выполнена");
        var inputStatus = this.GetInt(1, 2);
        if (inputStatus == 1) return true;
        else return false;
    }

    public int GetInt(int minValue, int maxValue)
    {
        Console.WriteLine($"Введите число от {minValue} до {maxValue}: ");
        while (true)
        {
            int number;
            string? input = Console.ReadLine();
            if (int.TryParse(input, out number))
                if (number >= minValue && number <= maxValue)
                    return number;
            Console.WriteLine("Ошибка ввода - Вы ввели недопустимое значение.\nПовторите ввод: ");
        }
    }
    public int GetTaskId()
    {
        Console.WriteLine("Введите Id задачи: ");
        while (true)
        {
            int number;
            string? input = Console.ReadLine();
            if (int.TryParse(input, out number))
                return number;
            Console.WriteLine("Ошибка ввода - Вы ввели недопустимое значение.\nПовторите ввод: ");
        }
    }

    public DateTime GetCreatedAt()
    {
        Console.WriteLine("Дата и время создания задачи:");
        Console.WriteLine("1 - Текущая дата и время\n2 - Ввести в ручную");
        if (this.GetInt(1, 2) == 1)
            return DateTime.Now;
        else
            return this.GetInputDataTime();
    }

    private DateTime GetInputDataTime()
    {
        while (true)
        {
            Console.Write("Введите дату и время (гггг-мм-дд чч:мм:сс): ");
            string? input = Console.ReadLine();
        
            if (DateTime.TryParse(input, out DateTime result))
            {
                return result;
            }
            Console.WriteLine("Неверный формат. Попробуйте снова.");
        }
    }
}
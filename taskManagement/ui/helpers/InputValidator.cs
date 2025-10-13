using Microsoft.IdentityModel.Tokens;

namespace taskManagement.ui.helpers;

public class InputValidator
{
    public static string? GetString(string inputDescription, bool isOptional = false)
    {
        string ruleMessage = "Строка не может быть пустой";
        while (true)
        {
            Console.WriteLine($"Введите {inputDescription}");
            string? input = Console.ReadLine();
            if (!input.IsNullOrEmpty())
                return input.Trim();
            else if (!isOptional)
                Console.WriteLine(ruleMessage);
            else
                return null;
        }
    }

    public static int GetNumberBetween(int minValue, int maxValue)
    {
        Console.WriteLine($"Введите число от {minValue} до {maxValue}: ");
        while (true)
        {
            int number;
            string? input = Console.ReadLine();
            if (int.TryParse(input, out number) && number >= minValue && number <= maxValue)
                    return number;
            Console.WriteLine("Ошибка ввода - Вы ввели недопустимое значение.\nПовторите ввод: ");
        }
    }
    
    public static int GetNumber(string message)
    {
        Console.WriteLine(message);
        while (true)
        {
            int number;
            string? input = Console.ReadLine();
            if (int.TryParse(input, out number))
                return number;
            Console.WriteLine("Ошибка ввода - Вы ввели недопустимое значение.\nПовторите ввод: ");
        }
    }

    public static DateTime GetInputDataTime()
    {
        while (true)
        {
            Console.Write("Введите дату и время (гггг-мм-дд чч:мм:сс): ");
            string? input = Console.ReadLine();
        
            if (DateTime.TryParse(input, out DateTime result))
                return result;
            Console.WriteLine("Неверный формат. Попробуйте снова.");
        }
    }
}
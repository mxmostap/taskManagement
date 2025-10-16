using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using taskManagement.database;
using taskManagement.database.implementation;
using taskManagement.repositories;
using taskManagement.repositories.implementation;
using taskManagement.services;
using taskManagement.services.implementation;
using taskManagement.UI;
using taskManagement.ui.implementation;

namespace taskManagement;

class Program
{
    private static ServiceProvider _serviceProvider;
    
    static async Task Main(string[] args)
    {
        try
        {
            SetupDI();
            await InitializeDatabaseAsync();
            await RunApplicationAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    static void SetupDI()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .Build();

        var connectionString = configuration["MsSQLSettings:ConnectionString"];

        var services = new ServiceCollection();
        
        services.AddSingleton<IDatabaseConnectionFactory>(
            new SqlServerConnectionFactory(connectionString));

        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<ITasksService, TasksService>();
        services.AddScoped<IConsoleMenu, ConsoleMenu>();
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    static async Task InitializeDatabaseAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
        
        Console.WriteLine("Инициализация базы данных запущена.");
        
        if (!await databaseInitializer.TestConnectionAsync())
        {
            throw new Exception("Ошибка подключения к базе данных, проверьте строку подключения.");
        }
        
        await databaseInitializer.InitializeAsync();
        Console.WriteLine("База данных инициализирована. Нажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();
    }
    
    static async Task RunApplicationAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var consoleMenu = scope.ServiceProvider.GetRequiredService<IConsoleMenu>();
        await consoleMenu.ShowMainMenuAsync();
    }
}
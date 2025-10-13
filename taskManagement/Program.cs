using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using taskManagement.database;
using taskManagement.database.impl;
using taskManagement.repository;
using taskManagement.repository.impl;
using taskManagement.service;
using taskManagement.service.impl;
using taskManagement.UI;
using taskManagement.UI.impl;

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
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

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
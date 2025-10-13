namespace taskManagement.database;

public interface IDatabaseInitializer
{
    Task<bool> TestConnectionAsync();
    Task InitializeAsync();
}
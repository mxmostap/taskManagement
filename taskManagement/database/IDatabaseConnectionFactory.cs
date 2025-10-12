using System.Data;

namespace taskManagement.database;

public interface IDatabaseConnectionFactory
{
    IDbConnection GetConnection();
}
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TaskWSS.DatabaseContext;

namespace TaskWSS.Tests.DbContext;

public class SqLiteConnectionFactory : IDisposable
{
    private bool _disposedValue = false;

    public static UnitTestDbContext CreateContextForSQLite()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var option = new DbContextOptionsBuilder<TaskDatabaseContext>().UseSqlite(connection).Options;

        var context = new UnitTestDbContext(option);

        if (context != null)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        return context;
    }
    

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
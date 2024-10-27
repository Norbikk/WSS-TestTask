using TaskWSS.Models;

namespace TaskWSS.DatabaseContext;

public class DbInitializer : IDisposable
{
    private readonly TaskDatabaseContext _context;
    private readonly ILogger<DbInitializer> _logger;
    private bool _disposed = false;
    
    public DbInitializer(TaskDatabaseContext context, 
        ILogger<DbInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Initialize()
    {
        CreateDefaultCompanies();
    }

    private void CreateDefaultCompanies()
    {
        try
        {
            if(!_context.Companies.Any())
            {
                var company = new Company
                {
                    Name = "Company 1",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name = "Department 1",
                            UnitDepartments = new List<UnitDepartment>
                            {
                                new UnitDepartment
                                {
                                    Name = "Unit Department 1"
                                }
                            }
                        }
                    }
                }; 
                
                _context.Companies.Add(company);
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while initializing database");
        }
    }

    public void Dispose()
    {
        if(_disposed)
        {
            return;
        }
        
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }
}
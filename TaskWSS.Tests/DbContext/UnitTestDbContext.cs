using Microsoft.EntityFrameworkCore;
using TaskWSS.DatabaseContext;
using TaskWSS.Models;

namespace TaskWSS.Tests.DbContext;

public class UnitTestDbContext : TaskDatabaseContext
{
    public UnitTestDbContext(DbContextOptions<TaskDatabaseContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

    public void InitializeDbContext()
    {
        this.Companies.Add(new Company { Id = 1, Name = "Test Company" });
        this.Departments.Add(new Department() { Id = 1, Name = "Test Department", CompanyId = 1 });
        this.UnitDepartments.Add(new UnitDepartment() { Id = 1, Name = "Test Unit", DepartmentId = 1 });
        this.SaveChanges();
    }
}
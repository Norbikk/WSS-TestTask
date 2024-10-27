using Microsoft.EntityFrameworkCore;
using TaskWSS.Models;

namespace TaskWSS.DatabaseContext;

public class TaskDatabaseContext : DbContext
{
    public virtual DbSet<Company> Companies { get; set; }
    
    public virtual DbSet<Department> Departments { get; set; }
    
    public virtual DbSet<UnitDepartment> UnitDepartments { get; set; }
    
    public TaskDatabaseContext(DbContextOptions<TaskDatabaseContext> options):base(options)
    {
    }
    
    internal TaskDatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>()
            .HasMany(c => c.Departments)
            .WithOne(d => d.Company)
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Department>()
            .HasMany(d => d.UnitDepartments)
            .WithOne(ud => ud.Department)
            .HasForeignKey(ud => ud.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}
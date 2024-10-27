using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskWSS.AutoMapper;
using TaskWSS.Interfaces;
using TaskWSS.Operations;
using TaskWSS.Services;
using TaskWSS.Tests.DbContext;
using TaskWSS.ViewModels;

namespace TaskWSS.Tests.Services;

public class DepartmentServiceTests
{
    private readonly UnitTestDbContext _context;
    private readonly IDepartmentService _departmentService;

    public DepartmentServiceTests()
    {
        _context = SqLiteConnectionFactory.CreateContextForSQLite();
        _context.InitializeDbContext();
        
        IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new DepartmentProfile())));
        _departmentService = new DepartmentService(_context, mapper);
    }
    
    [Fact]
    public async Task GetDepartmentsAsync_ShouldReturnDepartments()
    {
        // Act
        var result = await _departmentService.GetDepartmentsAsync();

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Single(result.Result);
    }

    [Fact]
    public async Task GetDepartmentAsync_ShouldReturnDepartment()
    {
        // Arrange
        int id = 1;
        
        // Act
        var result = await _departmentService.GetDepartmentAsync(id);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.NotNull(result.Result);
    }

    [Fact]
    public async Task GetDepartmentAsync_ShouldReturnNotFound()
    {
        // Arrange
        int id = 5;
        
        // Act
        var result = await _departmentService.GetDepartmentAsync(id);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Департамент не найден", result.Exception.Message);
    }

    [Fact]
    public async Task CreateDepartmentAsync_ShouldReturnDepartmentId()
    {
        // Arrange
        int companyId = 1;
        int nextId = 2;
        
        var departmentRequest = new CreateDepartmentRequest { Name = "New Department", CompanyId = companyId };
        
        // Act
        var result = await _departmentService.CreateDepartmentAsync(departmentRequest);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(nextId, result.Result);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldReturnDepartmentId()
    {
        // Arrange
        int id = 1;
        int companyId = 1;
        
        var departmentRequest = new UpdateDepartmentRequest { Name = "Updated Department", CompanyId = companyId};
        var department = await _context.Departments.FirstAsync();
        string oldName = department.Name;
        
        // Act
        var result = await _departmentService.UpdateDepartmentAsync(id, departmentRequest);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(id, result.Result);
        Assert.NotEqual(oldName, department.Name);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldReturnNotFoundDepartment()
    {
        // Arrange
        int id = 5;
        int companyId = 1;
        
        var departmentRequest = new UpdateDepartmentRequest { Name = "Updated Department", CompanyId = companyId};
        
        // Act
        var result = await _departmentService.UpdateDepartmentAsync(id, departmentRequest);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Департамент не найден", result.Exception.Message);
    }
    
    [Fact]
    public async Task UpdateDepartmentAsync_ShouldReturnNotFoundCompany()
    {
        // Arrange
        int id = 1;
        int companyId = 5;
        
        var departmentRequest = new UpdateDepartmentRequest { Name = "Updated Department", CompanyId = companyId};
        
        // Act
        var result = await _departmentService.UpdateDepartmentAsync(id, departmentRequest);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Компания не найдена", result.Exception.Message);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ShouldReturnDepartmentId()
    {
        // Arrange
        int id = 1;
        
        // Act
        var result = await _departmentService.DeleteDepartmentAsync(id);
        var department = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(id, result.Result);
        Assert.Null(department);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ShouldReturnNotFound()
    {
        // Arrange
        int id = 5;
        
        // Act
        var result = await _departmentService.DeleteDepartmentAsync(id);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Департамент не найден", result.Exception.Message);
    }
}
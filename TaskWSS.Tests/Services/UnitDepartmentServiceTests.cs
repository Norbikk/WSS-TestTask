using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskWSS.AutoMapper;
using TaskWSS.Interfaces;
using TaskWSS.Operations;
using TaskWSS.Services;
using TaskWSS.Tests.DbContext;
using TaskWSS.ViewModels;

namespace TaskWSS.Tests.Services;

public class UnitDepartmentServiceTests
{
    private readonly UnitTestDbContext _context;
    private readonly IUnitDepartmentService _unitDepartmentService;
    
    public UnitDepartmentServiceTests()
    {
        _context = SqLiteConnectionFactory.CreateContextForSQLite();
        _context.InitializeDbContext();
        
        IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new UnitDepartmentProfile())));
        _unitDepartmentService = new UnitDepartmentService(_context, mapper);
    }
    
    [Fact]
    public async Task GetUnitDepartmentsAsync_ShouldReturnUnitDepartments()
    {
        // Act
        var result = await _unitDepartmentService.GetUnitDepartmentsAsync();

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Single(result.Result);
    }

    [Fact]
    public async Task GetUnitDepartmentAsync_ShouldReturnUnitDepartment()
    {
        // Arrange
        int id = 1;
        
        // Act
        var result = await _unitDepartmentService.GetUnitDepartmentAsync(id);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.NotNull(result.Result);
    }

    [Fact]
    public async Task GetUnitDepartmentAsync_ShouldReturnNotFound()
    {
        // Arrange
        int id = 5;
        
        // Act
        var result = await _unitDepartmentService.GetUnitDepartmentAsync(id);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Отдел не найден", result.Exception.Message);
    }

    [Fact]
    public async Task CreateUnitDepartmentAsync_ShouldReturnUnitDepartmentId()
    {
        // Arrange
        int departmentId = 1;
        int nextId = 2;
        
        var unitDepartmentRequest = new CreateUnitDepartmentRequest { Name = "New Unit", DepartmentId = departmentId };
        
        // Act
        var result = await _unitDepartmentService.CreateUnitDepartmentAsync(unitDepartmentRequest);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(nextId, result.Result);
    }
    
    [Fact]
    public async Task CreateUnitDepartmentAsync_ShouldReturnNotFoundDepartment()
    {
        // Arrange
        int departmentId = 5;
        
        var unitDepartmentRequest = new CreateUnitDepartmentRequest { Name = "New Unit", DepartmentId = departmentId };
        
        // Act
        var result = await _unitDepartmentService.CreateUnitDepartmentAsync(unitDepartmentRequest);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Департамент не найден", result.Exception.Message);
    }

    [Fact]
    public async Task UpdateUnitDepartmentAsync_ShouldReturnUnitDepartmentId()
    {
        // Arrange
        int id = 1;
        int departmentId = 1;
        
        var unitDepartmentRequest = new UpdateUnitDepartmentRequest { Name = "Updated Unit", DepartmentId = departmentId};

        var unitDepartment = await _context.UnitDepartments.FirstAsync(x => x.Id == id);
        string oldName = unitDepartment.Name;
        
        // Act
        var result = await _unitDepartmentService.UpdateUnitDepartmentAsync(id, unitDepartmentRequest);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(1, result.Result);
        Assert.NotEqual(oldName, unitDepartment.Name);
    }

    [Fact]
    public async Task UpdateUnitDepartmentAsync_ShouldReturnNotFoundUnit()
    {
        // Arrange
        int id = 5;
        int departmentId = 1;
        
        var unitDepartmentRequest = new UpdateUnitDepartmentRequest { Name = "Updated Unit", DepartmentId = departmentId};
        
        // Act
        var result = await _unitDepartmentService.UpdateUnitDepartmentAsync(id, unitDepartmentRequest);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Отдел не найден", result.Exception.Message);
    }
    
    [Fact]
    public async Task UpdateUnitDepartmentAsync_ShouldReturnNotFoundDepartment()
    {
        // Arrange
        int id = 1;
        int departmentId = 5;
        
        var unitDepartmentRequest = new UpdateUnitDepartmentRequest { Name = "Updated Unit", DepartmentId = departmentId};
        
        // Act
        var result = await _unitDepartmentService.UpdateUnitDepartmentAsync(id, unitDepartmentRequest);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Департамент не найден", result.Exception.Message);
    }

    [Fact]
    public async Task DeleteUnitDepartmentAsync_ShouldReturnUnitDepartmentId()
    {
        // Arrange
        int id = 1;
        
        // Act
        var result = await _unitDepartmentService.DeleteUnitDepartmentAsync(id);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(1, result.Result);
    }

    [Fact]
    public async Task DeleteUnitDepartmentAsync_ShouldReturnNotFound()
    {
        // Arrange
        int id = 5;
        
        // Act
        var result = await _unitDepartmentService.DeleteUnitDepartmentAsync(id);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Отдел не найден", result.Exception.Message);
    }
}
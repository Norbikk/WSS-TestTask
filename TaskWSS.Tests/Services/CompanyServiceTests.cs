using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskWSS.AutoMapper;
using TaskWSS.Interfaces;
using TaskWSS.Models;
using TaskWSS.Operations;
using TaskWSS.Services;
using TaskWSS.Tests.DbContext;
using TaskWSS.ViewModels;
using TaskWSS.ViewModels.ImportModels;

namespace TaskWSS.Tests.Services;

public class CompanyServiceTests
{
    private readonly UnitTestDbContext _context;
    private readonly ICompanyService _companyService;

    public CompanyServiceTests()
    {
        _context = SqLiteConnectionFactory.CreateContextForSQLite();
        _context.InitializeDbContext();
        
        IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new CompanyProfile())));
        _companyService = new CompanyService(_context, mapper);
    }
    
    [Fact]
    public async Task GetCompaniesAsync_ShouldReturn_Companies()
    {
        // Act
        var result = await _companyService.GetCompaniesAsync();

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Single(result.Result);
    }
    
    
    [Fact]
    public async Task GetCompanyAsync_ShouldReturnCompany()
    {
        // Arrange
        int id = 1;
        
        // Act
        var result = await _companyService.GetCompanyAsync(id);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.NotNull(result.Result);
    }

    [Fact]
    public async Task GetCompanyAsync_ShouldReturnNotFound()
    {
        // Arrange
        int id = 5;
        
        // Act
        var result = await _companyService.GetCompanyAsync(id);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Компания не найдена", result.Exception.Message);
    }

    [Fact]
    public async Task CreateCompanyAsync_ShouldReturnCompanyId()
    {
        // Arrange
        var companyRequest = new CreateCompanyRequest { Name = "New Company" };
        int nextId = 2;

        // Act
        var result = await _companyService.CreateCompanyAsync(companyRequest);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(nextId, result.Result);
    }
    
    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnCompanyId()
    {
        // Arrange
        var companyRequest = new UpdateCompanyRequest { Name = "Updated Company" };
        var company = await _context.Companies.FirstAsync();
        string oldName = company.Name;

        // Act
        var result = await _companyService.UpdateCompanyAsync(1, companyRequest);
        
        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(company.Id, result.Result);
        Assert.NotEqual(oldName, company.Name);
    }

    [Fact]
    public async Task UpdateCompanyAsync_ShouldReturnNotFound()
    {
        // Arrange
        var companyRequest = new UpdateCompanyRequest { Name = "Updated Company" };

        // Act
        var result = await _companyService.UpdateCompanyAsync(5, companyRequest);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Компания не найдена", result.Exception.Message);
    }

    [Fact]
    public async Task DeleteCompanyAsync_ShouldReturnCompanyId()
    {
        // Arrange
        var id = 1;
        var result = await _companyService.DeleteCompanyAsync(id);
        
        // Act
        var company = await _context.Companies.FirstOrDefaultAsync(x=>x.Id == id);
        
        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(id, result.Result);
        Assert.Null(company);
    }

    [Fact]
    public async Task DeleteCompanyAsync_ShouldReturnNotFound()
    {
        // Arrange
        int id = 5;
        
        // Act
        var result = await _companyService.DeleteCompanyAsync(id);

        // Assert
        Assert.Equal(StatusOperation.NotFound, result.Status);
        Assert.Equal("Компания не найдена", result.Exception.Message);
    }

    [Fact]
    public async Task ImportCompanyAsync_ShouldReturnCount()
    {
        // Arrange
        var companiesToCreate = new List<CompanyImportModel> { new CompanyImportModel { Name = "New Company" } };

        // Act
        var result = await _companyService.ImportCompanyAsync(companiesToCreate);

        // Assert
        Assert.Equal(StatusOperation.Success, result.Status);
        Assert.Equal(1, result.Result);
    }


    [Fact]
    public async Task IsCompanyExistAsync_ShouldReturnTrue()
    {
        // Act
        var result = await _companyService.IsCompanyExistAsync("Test Company");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsCompanyExistAsync_ShouldReturnFalse()
    {
        // Act
        var result = await _companyService.IsCompanyExistAsync("FalseName");

        // Assert
        Assert.False(result);
    }
}
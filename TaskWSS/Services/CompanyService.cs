using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskWSS.DatabaseContext;
using TaskWSS.Exceptions;
using TaskWSS.Interfaces;
using TaskWSS.Models;
using TaskWSS.Operations;
using TaskWSS.ViewModels;
using TaskWSS.ViewModels.ImportModels;

namespace TaskWSS.Services;

public class CompanyService : ICompanyService
{
    private readonly TaskDatabaseContext _dbContext;
    private readonly IMapper _mapper;
    
    public CompanyService(TaskDatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IOperationResult<List<Company>>> GetCompaniesAsync()
    {
        try
        {
            var companies = await _dbContext.Companies
                .Include(x=>x.Departments)
                .ThenInclude(x=>x.UnitDepartments)
                .ToListAsync();
            
            return OperationResult.Success(companies);
        }
        catch (Exception e)
        {
            return OperationResult.Error<List<Company>>(e);
        }
    }

    public async Task<IOperationResult<Company>> GetCompanyAsync(int id)
    {
        try
        {
            var company = await _dbContext.Companies
                .Include(x=>x.Departments)
                .ThenInclude(x=>x.UnitDepartments)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if(company is null)
            {
                return OperationResult.NotFound<Company>(new NotFoundException("Компания не найдена", id));
            }
            
            return OperationResult.Success(company);
        }
        catch (Exception e)
        { 
            return OperationResult.Error<Company>(e);
        }
    }

    public async Task<IOperationResult<int>> CreateCompanyAsync(CreateCompanyRequest companyRequest)
    {
        try
        {
            var company = _mapper.Map<Company>(companyRequest);
            
            _dbContext.Companies.Add(company);
            await _dbContext.SaveChangesAsync();
            
            return OperationResult.Success(company.Id);

        }
        catch (Exception e)
        {
            return OperationResult.Error<int>(e);
        }
    }

    public async Task<IOperationResult<int>> UpdateCompanyAsync(int id, UpdateCompanyRequest companyRequest)
    {
        try
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if(company is null)
            {
                return OperationResult.NotFound<int>(new NotFoundException("Компания не найдена", id));
            }
            
            _mapper.Map(companyRequest, company);
            
            _dbContext.Companies.Update(company);
            await _dbContext.SaveChangesAsync();
            
            return OperationResult.Success(company.Id);
        }
        catch (Exception e)
        {
            return OperationResult.Error<int>(e);
        }
    }

    public async Task<IOperationResult<int>> DeleteCompanyAsync(int id)
    {
        try
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if(company is null)
            {
                return OperationResult.NotFound<int>(new NotFoundException("Компания не найдена", id));
            }
            
            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();
            
            return OperationResult.Success(company.Id);
        }
        catch (Exception e)
        {
            return OperationResult.Error<int>(e);
        }
    }
    
    public async Task<IOperationResult<int>> ImportCompanyAsync(List<CompanyImportModel> companiesToCreate)
    {
        try
        {
            var companies = _mapper.Map<List<Company>>(companiesToCreate);
            
            await _dbContext.Companies.AddRangeAsync(companies);
            await _dbContext.SaveChangesAsync();
            
            return OperationResult.Success(companies.Count);
            
        }
        catch (Exception e)
        {
            return OperationResult.Error<int>(e);
        }
    }
    public async Task<bool> IsCompanyExistAsync(string name)
    {
        return await _dbContext.Companies.AnyAsync(x => x.Name == name);
    }

}
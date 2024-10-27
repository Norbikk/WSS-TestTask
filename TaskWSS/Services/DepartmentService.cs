using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskWSS.DatabaseContext;
using TaskWSS.Exceptions;
using TaskWSS.Interfaces;
using TaskWSS.Models;
using TaskWSS.Operations;
using TaskWSS.ViewModels;

namespace TaskWSS.Services;

public class DepartmentService : IDepartmentService
{
    private readonly TaskDatabaseContext _dbContext;
    private readonly IMapper _mapper;
    
    public DepartmentService(TaskDatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IOperationResult<List<Department>>> GetDepartmentsAsync()
    {
        try
        {
            var departments = await _dbContext.Departments
                .Include(x=>x.Company)
                .Include(x=>x.UnitDepartments)
                .ToListAsync();
            
            return OperationResult.Success(departments);
        }
        catch (Exception e)
        {
            return OperationResult.Error<List<Department>>(e);
        }
    }

    public async Task<IOperationResult<Department>> GetDepartmentAsync(int id)
    {
        try
        {
            var department = await _dbContext.Departments
                .Include(x=>x.Company)
                .Include(x=>x.UnitDepartments)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if(department is null)
            {
                return OperationResult.NotFound<Department>(new NotFoundException("Департамент не найден", id));
            }
            
            return OperationResult.Success(department);
        }
        catch (Exception e)
        {
            return OperationResult.Error<Department>(e);
        }
       
    }
    
    public async Task<IOperationResult<List<Department>>> GetDepartmentsByCompanyIdAsync(int companyId)
    {
        try
        {
            var company = await _dbContext.Companies
                .Include(x => x.Departments)
                .ThenInclude(x=>x.UnitDepartments)
                .FirstOrDefaultAsync(x => x.Id == companyId);
            
            if(company is null)
            {
                return OperationResult.NotFound<List<Department>>(new NotFoundException("Компания не найдена", companyId));
            }
            
            return OperationResult.Success(company.Departments);
        }
        catch (Exception e)
        {
            return OperationResult.Error<List<Department>>(e);  
        }
    }

    public async Task<IOperationResult<int>> CreateDepartmentAsync(CreateDepartmentRequest request)
    {
        try
        {
            if (!await IsCompanyExists(request.CompanyId))
            {
                return OperationResult.NotFound<int>(new NotFoundException("Компания не найдена", request.CompanyId));
            }
            
            var department = _mapper.Map<Department>(request);
            
            _dbContext.Departments.Add(department);
            await _dbContext.SaveChangesAsync();
            
            return OperationResult.Success(department.Id);
            
        }
        catch (Exception e)
        {
            return OperationResult.Error<int>(e);
        }
    }

    public async Task<IOperationResult<int>> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request)
    {
        try
        {
            var department = _dbContext.Departments.FirstOrDefault(x => x.Id == id);
            
            if(department is null)
            {
                return OperationResult.NotFound<int>(new NotFoundException("Департамент не найден", id));
            }
            
            if(!await IsCompanyExists(request.CompanyId))
            {
                return OperationResult.NotFound<int>(new NotFoundException("Компания не найдена", request.CompanyId));
            }
            
            _mapper.Map(request, department);
            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync();

            return OperationResult.Success(department.Id);
        }
        catch (Exception e)
        {
            return OperationResult.Error<int>(e);
        }
    }

    public async Task<IOperationResult<int>> DeleteDepartmentAsync(int id)
    {
        try
        {
            var department = _dbContext.Departments.FirstOrDefault(x => x.Id == id);
            
            if(department is null)
            {
                return OperationResult.NotFound<int>(new NotFoundException("Департамент не найден", id));
            }
            
            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync();
            
            return OperationResult.Success(id);
        }
        catch (Exception e)
        {
            return OperationResult.Error<int>(e);
        }
    }
    
    private async Task<bool> IsCompanyExists(int companyId)
    {
        return await _dbContext.Companies.AnyAsync(x => x.Id == companyId);
    }
}
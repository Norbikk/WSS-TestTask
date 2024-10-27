using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskWSS.DatabaseContext;
using TaskWSS.Exceptions;
using TaskWSS.Interfaces;
using TaskWSS.Models;
using TaskWSS.Operations;
using TaskWSS.ViewModels;
using TaskWSS.ViewModels.Response;

namespace TaskWSS.Services;

public class UnitDepartmentService : IUnitDepartmentService
{
    private readonly TaskDatabaseContext _dbContext;
    private readonly IMapper _mapper;

    public UnitDepartmentService(TaskDatabaseContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IOperationResult<List<UnitDepartment>>> GetUnitDepartmentsAsync()
    {
        try
        {
            var unitDepartments = await _dbContext.UnitDepartments
                .Include(x=>x.Department)
                .ThenInclude(x=>x.Company)
                .ToListAsync();
            
            return OperationResult.Success(unitDepartments);
        }
        catch (Exception e)
        {
            return OperationResult.Error<List<UnitDepartment>>(e);
        }
    }
    
    public async Task<IOperationResult<List<UnitDepartment>>> GetUnitDepartmentsByDepartmentIdAsync(int departmentId)
    {
        try
        {
            var department = await _dbContext.Departments
                .Include(x=>x.UnitDepartments)
                .FirstOrDefaultAsync(x => x.Id == departmentId);
            
            if (department is null)
            {
                return OperationResult.NotFound<List<UnitDepartment>>(new NotFoundException("Департамент не найден", departmentId));
            }
            
            var result = department.UnitDepartments
                .Select(x => _mapper.Map<UnitDepartment>(x))
                .ToList();

            return OperationResult.Success(department.UnitDepartments);

        }
        catch (Exception e)
        {
            return OperationResult.Error<List<UnitDepartment>>(e);
        }
    }

    public async Task<IOperationResult<UnitDepartment>> GetUnitDepartmentAsync(int id)
    {
        try
        {
            var unitDepartment = await _dbContext.UnitDepartments
                .Include(x=>x.Department)
                .ThenInclude(x=>x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (unitDepartment is null)
            {
                return OperationResult.NotFound<UnitDepartment>(new NotFoundException("Отдел не найден", id));
            }

            var result = _mapper.Map<UnitDepartmentResponse>(unitDepartment);
            
            return OperationResult.Success(unitDepartment);
        }
        catch (Exception e)
        {
            return OperationResult.Error<UnitDepartment>(e);
        }
    }

    public async Task<IOperationResult<int>> CreateUnitDepartmentAsync(CreateUnitDepartmentRequest createUnitDepartmentRequest)
    {
        try
        {
            if (!await IsDepartmentExist(createUnitDepartmentRequest.DepartmentId))
            {
                return OperationResult.NotFound<int>(new NotFoundException("Департамент не найден", createUnitDepartmentRequest.DepartmentId));
            }

            var result = _mapper.Map<UnitDepartment>(createUnitDepartmentRequest);
            
            _dbContext.UnitDepartments.Add(result);
            await _dbContext.SaveChangesAsync();
            
            return OperationResult.Success(result.Id);
        }
        catch (Exception e)
        {
            return OperationResult.Error<int>(e);
        }
    }

    public async Task<IOperationResult<int>> UpdateUnitDepartmentAsync(int unitDepartmentId, UpdateUnitDepartmentRequest updateUnitDepartmentRequest)
    {
        try
        {
            var unitDepartment = await _dbContext.UnitDepartments.FirstOrDefaultAsync(x => x.Id == unitDepartmentId);
            
            if (unitDepartment is null)
            {
                return OperationResult.NotFound<int>(new NotFoundException("Отдел не найден", unitDepartmentId));
            }
            if (!await IsDepartmentExist(updateUnitDepartmentRequest.DepartmentId))
            {
                return OperationResult.NotFound<int>(new NotFoundException("Департамент не найден", updateUnitDepartmentRequest.DepartmentId));
            }

            _mapper.Map(updateUnitDepartmentRequest, unitDepartment);
            
            _dbContext.UnitDepartments.Update(unitDepartment);
            await _dbContext.SaveChangesAsync();
            
            return OperationResult.Success(unitDepartment.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IOperationResult<int>> DeleteUnitDepartmentAsync(int id)
    {
        try
        {
            var unitDepartment = await _dbContext.UnitDepartments.FirstOrDefaultAsync(x => x.Id == id);
            if (unitDepartment is null)
            {
                return OperationResult.NotFound<int>(new NotFoundException("Отдел не найден", id));
            }

            _dbContext.UnitDepartments.Remove(unitDepartment);
            await _dbContext.SaveChangesAsync();
            
            return OperationResult.Success(id);
        }
        catch (Exception e)
        {
            return OperationResult.Error<int>(e);
        }
    }
    
    private async Task<bool> IsDepartmentExist(int departmentId)
    {
        return await _dbContext.Departments.AnyAsync(x => x.Id == departmentId);
    }
}
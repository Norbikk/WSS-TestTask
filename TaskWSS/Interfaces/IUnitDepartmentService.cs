using TaskWSS.Models;
using TaskWSS.Operations;
using TaskWSS.ViewModels;

namespace TaskWSS.Interfaces;

public interface IUnitDepartmentService
{
    /// <summary>
    /// Получение всех отделов
    /// </summary>
    /// <returns></returns>
    Task<IOperationResult<List<UnitDepartment>>> GetUnitDepartmentsAsync();
    
    /// <summary>
    /// Получение определенного отдела
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IOperationResult<UnitDepartment>> GetUnitDepartmentAsync(int id);
    
    /// <summary>
    /// Вывод всех отделов по ID департамента
    /// </summary>
    /// <param name="departmentId"></param>
    /// <returns></returns>
    Task<IOperationResult<List<UnitDepartment>>> GetUnitDepartmentsByDepartmentIdAsync(int departmentId);
    
    /// <summary>
    /// Создание отдела
    /// </summary>
    /// <param name="createUnitDepartmentRequest"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> CreateUnitDepartmentAsync(CreateUnitDepartmentRequest createUnitDepartmentRequest);
    
    /// <summary>
    /// Обновление отдела
    /// </summary>
    /// <param name="unitDepartmentId"></param>
    /// <param name="updateUnitDepartmentRequest"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> UpdateUnitDepartmentAsync(int unitDepartmentId, UpdateUnitDepartmentRequest updateUnitDepartmentRequest);
    
    /// <summary>
    /// Удаление отдела
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> DeleteUnitDepartmentAsync(int id);
}
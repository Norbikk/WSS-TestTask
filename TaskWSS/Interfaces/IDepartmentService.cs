using TaskWSS.Models;
using TaskWSS.Operations;
using TaskWSS.ViewModels;

namespace TaskWSS.Interfaces;

public interface IDepartmentService
{
    /// <summary>
    /// Получение всех департаментов
    /// </summary>
    /// <returns></returns>
    Task<IOperationResult<List<Department>>> GetDepartmentsAsync();
    
    /// <summary>
    /// Получение определенного департамента
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IOperationResult<Department>> GetDepartmentAsync(int id);
    
    /// <summary>
    /// Получение всех департаментов по ID компании
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<IOperationResult<List<Department>>> GetDepartmentsByCompanyIdAsync(int companyId);
    
    /// <summary>
    /// Создание департамента
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> CreateDepartmentAsync(CreateDepartmentRequest request);
    
    /// <summary>
    /// Обновление департамента
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request);
    
    /// <summary>
    /// Удаление департамента
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> DeleteDepartmentAsync(int id);
}
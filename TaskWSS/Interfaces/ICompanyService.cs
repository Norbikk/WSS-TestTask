using TaskWSS.Models;
using TaskWSS.Operations;
using TaskWSS.ViewModels;
using TaskWSS.ViewModels.ImportModels;

namespace TaskWSS.Interfaces;

public interface ICompanyService
{
    /// <summary>
    /// Получение всех компаний
    /// </summary>
    /// <returns></returns>
    Task<IOperationResult<List<Company>>> GetCompaniesAsync();
    
    /// <summary>
    /// Получение определенной компании
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IOperationResult<Company>> GetCompanyAsync(int id);
    
    /// <summary>
    /// Создание новой компании
    /// </summary>
    /// <param name="companyRequest"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> CreateCompanyAsync(CreateCompanyRequest companyRequest);
    
    /// <summary>
    /// Обновление компании
    /// </summary>
    /// <param name="id"></param>
    /// <param name="companyRequest"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> UpdateCompanyAsync(int id, UpdateCompanyRequest companyRequest);
    
    /// <summary>
    /// Удаление компании
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> DeleteCompanyAsync(int id);

    /// <summary>
    /// Флаг, указывающий есть ли компания с таким именем
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> IsCompanyExistAsync(string name);

    /// <summary>
    /// Создание компаний из CompanyImportModel
    /// </summary>
    /// <param name="companiesToCreate"></param>
    /// <returns></returns>
    Task<IOperationResult<int>> ImportCompanyAsync(List<CompanyImportModel> companiesToCreate);
}
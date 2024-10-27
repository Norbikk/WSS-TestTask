using TaskWSS.Operations;

namespace TaskWSS.Interfaces;

public interface ICompanyImportService
{
    /// <summary>
    /// Импорт компаний из XML
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<OperationResult> ImportCompaniesFromXmlAsync(IFormFile file);
}
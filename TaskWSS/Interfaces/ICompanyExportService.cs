using TaskWSS.Operations;

namespace TaskWSS.Interfaces;

public interface ICompanyExportService
{
    /// <summary>
    /// Экспорт компаний в XML
    /// </summary>
    /// <returns></returns>
    Task<IOperationResult<Stream>> ExportCompaniesToXmlAsync();
}
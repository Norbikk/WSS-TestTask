using System.Text;
using AutoMapper;
using TaskWSS.Helpers;
using TaskWSS.Interfaces;
using TaskWSS.Operations;
using TaskWSS.ViewModels.ImportModels;

namespace TaskWSS.Services.XmlService;

public class ExportXmlService : ICompanyExportService
{
    private readonly ICompanyService _companyService;
    private readonly IMapper _mapper;

    public ExportXmlService(ICompanyService companyService, IMapper mapper)
    {
        _companyService = companyService;
        _mapper = mapper;
    }

    public async Task<IOperationResult<Stream>> ExportCompaniesToXmlAsync()
    {
        try
        {
            var companiesResult = await _companyService.GetCompaniesAsync();
            if (companiesResult.Status != StatusOperation.Success)
            {
                return OperationResult.Error<Stream>(new Exception("Не удалось получить компании"));
            }

            var importModels = _mapper.Map<List<CompanyImportModel>>(companiesResult.Result);
            string xmlCompanies =
                XmlHelper.SerializeToXml(importModels);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlCompanies));

            return OperationResult.Success(stream);
        }
        catch (Exception e)
        {
            return OperationResult.Error<Stream>(e);
        }
    }
}
using AutoMapper;
using TaskWSS.Exceptions;
using TaskWSS.Helpers;
using TaskWSS.Interfaces;
using TaskWSS.Models;
using TaskWSS.Operations;
using TaskWSS.ViewModels.ImportModels;
using TaskWSS.ViewModels.Response;

namespace TaskWSS.Services.XmlService;

public class ImportXmlService : ICompanyImportService
{
    private readonly ICompanyService _companyService;

    public ImportXmlService(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<OperationResult> ImportCompaniesFromXmlAsync(IFormFile file)
    {
        try
        {
            await using (var stream = file.OpenReadStream())
            {
                var companies = await XmlHelper.DeserializeAsync<List<CompanyImportModel>>(stream);

                if (companies is null)
                {
                    return OperationResult.Error(new Exception("Не удалось десериализовать xml"));
                }
                
                var companiesToCreate = new List<CompanyImportModel>();
                foreach (var company in companies)
                {
                    bool exists = await _companyService.IsCompanyExistAsync(company.Name);
                    if (!exists)
                    {
                        companiesToCreate.Add(company);
                    }
                }
                
                if(companiesToCreate.Count == 0)
                {
                    return OperationResult.Error(new Exception("Все компании уже существуют"));
                }
                
                var companiesCreated = await _companyService.ImportCompanyAsync(companiesToCreate);
                
                if(companiesToCreate.Count != companiesCreated.Result)
                {
                    return OperationResult.Error(new Exception("Не все компании были созданы"));
                }
                
                return OperationResult.Success();
            }
        }
        catch (Exception e)
        {
            return OperationResult.Error(e);
        }
    }

}
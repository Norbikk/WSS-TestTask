using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskWSS.Interfaces;
using TaskWSS.Operations;
using TaskWSS.ViewModels;
using TaskWSS.ViewModels.Response;

namespace TaskWSS.ApiControllers;

[Route("api/v0")]
[Consumes(MediaTypeNames.Application.Json, "multipart/form-data")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Company API")]
[ApiController]
public class CompanyApiController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly ICompanyImportService _companyImportService;
    private readonly ICompanyExportService _companyExportService;
    private readonly IMapper _mapper;
    
    public CompanyApiController(ICompanyService companyService, 
        ICompanyImportService companyImportService, 
        ICompanyExportService companyExportService,
        IMapper mapper)
    {
        _companyService = companyService;
        _mapper = mapper;
        _companyImportService = companyImportService;
        _companyExportService = companyExportService;
    }
    
    /// <summary>
    /// Api метод для получения всех компаний
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [HttpGet("companies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCompaniesAsync()
    {
        var result = await _companyService.GetCompaniesAsync();

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                var companiesViewModel = _mapper.Map<List<CompanyResponse>>(result.Result);
                return Ok(companiesViewModel);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Api метод для получения определенной компании
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [HttpGet("company/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyAsync(int id)
    {
        var result = await _companyService.GetCompanyAsync(id);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                var companyViewModel = _mapper.Map<CompanyResponse>(result.Result);
                return Ok(companyViewModel);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Api метод для создания новой компании
    /// </summary>
    /// <param name="companyRequest"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [HttpPost("company")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyRequest companyRequest)
    {
        var result = await _companyService.CreateCompanyAsync(companyRequest);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.Success:
                return Ok($"Компания создана ID: {result.Result}");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Api метод для обновления компании
    /// </summary>
    /// <param name="id"></param>
    /// <param name="companyRequest"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [HttpPut("company/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompanyAsync(int id, [FromBody] UpdateCompanyRequest companyRequest)
    {
        var result = await _companyService.UpdateCompanyAsync(id, companyRequest);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                return Ok($"Компания обновлена ID: {result.Result}");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Api метод для удаления компании
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [HttpDelete("company/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompanyAsync(int id)
    {
        var result = await _companyService.DeleteCompanyAsync(id);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                return Ok($"Компания удалена ID: {result.Result}");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Api метод для импорта компаний из XML
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [HttpPost("company/import")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportCompaniesAsync(IFormFile file)
    {
        var result = await _companyImportService.ImportCompaniesFromXmlAsync(file);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.Success:
                return Ok("Компании импортированы");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Api метод для экспорта компаний в XML
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [HttpGet("company/export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExportCompaniesAsync()
    {
        var result = await _companyExportService.ExportCompaniesToXmlAsync();

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.Success:
                return File(result.Result, MediaTypeNames.Application.Xml, "companies.xml");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskWSS.Interfaces;
using TaskWSS.Operations;
using TaskWSS.ViewModels;
using TaskWSS.ViewModels.Response;

namespace TaskWSS.ApiControllers;

[Route("api/v0")]
[Consumes(MediaTypeNames.Application.Json, "multipart/form-data")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Department API")]
[ApiController]
public class DepartmentApiController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;

    public DepartmentApiController(IDepartmentService departmentService, IMapper mapper)
    {
        _departmentService = departmentService;
        _mapper = mapper;
    }
    
    [HttpGet("departments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDepartmentsAsync()
    {
        var result = await _departmentService.GetDepartmentsAsync();
        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.Success:
                var departments = _mapper.Map<List<DepartmentResponse>>(result.Result);
                return Ok(departments);
            default:
                throw new ArgumentOutOfRangeException();
        }
       
    }
    
    [HttpGet("department/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartmentAsync(int id)
    {
        var result = await _departmentService.GetDepartmentAsync(id);
        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.Success:
                var department = _mapper.Map<DepartmentResponse>(result.Result);
                return Ok(department);
            case StatusOperation.NotFound:
                return NotFound();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpGet("departments-by-company/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartmentByCompanyIdAsync(int id)
    {
        var result = await _departmentService.GetDepartmentsByCompanyIdAsync(id);
        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.Success:
                var departments = _mapper.Map<List<DepartmentResponse>>(result.Result);
                return Ok(departments);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpPost("department")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmentRequest departmentViewModel)
    {
        var result = await _departmentService.CreateDepartmentAsync(departmentViewModel);
        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                return Ok($"Департамент создан ID:{result.Result}");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpPut("department/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDepartmentAsync(int id, [FromBody] UpdateDepartmentRequest departmentViewModel)
    {
        var result = await _departmentService.UpdateDepartmentAsync(id, departmentViewModel);
        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                return Ok($"Департамент обновлен ID:{result.Result}");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpDelete("department/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDepartmentAsync(int id)
    {
        var result = await _departmentService.DeleteDepartmentAsync(id);
        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                return Ok($"Департамент удален ID:{result.Result}");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
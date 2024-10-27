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
[Tags("Unit API")]
[ApiController]
public class UnitDepartmentApiController: ControllerBase
{
    private readonly IUnitDepartmentService _unitDepartmentService;
    private readonly IMapper _mapper;


    public UnitDepartmentApiController(IUnitDepartmentService unitDepartmentService, IMapper mapper)
    {
        _unitDepartmentService = unitDepartmentService;
        _mapper = mapper;
    }
    
    [HttpGet("unit-departments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUnitDepartments()
    {
        var result = await _unitDepartmentService.GetUnitDepartmentsAsync();

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.Success:
                var departmentViewModels = _mapper.Map<List<UnitDepartmentResponse>>(result.Result);
                return Ok(departmentViewModels);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpGet("unit-department/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnitDepartment(int id)
    {
        var result = await _unitDepartmentService.GetUnitDepartmentAsync(id);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                var departmentViewModel = _mapper.Map<UnitDepartmentResponse>(result.Result);
                return Ok(departmentViewModel);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpGet("unit-departments-by-department/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnitDepartmentByDepartment(int id)
    {
        var result = await _unitDepartmentService.GetUnitDepartmentsByDepartmentIdAsync(id);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                var departmentViewModels = _mapper.Map<List<UnitDepartmentResponse>>(result.Result);
                return Ok(departmentViewModels);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpPost("unit-department")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUnitDepartment([FromBody] CreateUnitDepartmentRequest request)
    {
        var result = await _unitDepartmentService.CreateUnitDepartmentAsync(request);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                return Ok($"Отдел создан ID: {result.Result}");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpPut("unit-department/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUnitDepartment(int id, [FromBody] UpdateUnitDepartmentRequest request)
    {
        var result = await _unitDepartmentService.UpdateUnitDepartmentAsync(id, request);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                return Ok($"Отдел обновлен ID: {result.Result}");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    [HttpDelete("unit-department/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUnitDepartment(int id)
    {
        var result = await _unitDepartmentService.DeleteUnitDepartmentAsync(id);

        switch (result.Status)
        {
            case StatusOperation.Exception:
                return BadRequest(result.Exception.Message);
            case StatusOperation.NotFound:
                return NotFound(result.Exception.Message);
            case StatusOperation.Success:
                return Ok($"Отдел удален ID: {result.Result}");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
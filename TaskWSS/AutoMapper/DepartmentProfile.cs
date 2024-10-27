using AutoMapper;
using TaskWSS.Models;
using TaskWSS.ViewModels;
using TaskWSS.ViewModels.ImportModels;
using TaskWSS.ViewModels.Response;

namespace TaskWSS.AutoMapper;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<CreateDepartmentRequest, Department>();
        CreateMap<UpdateDepartmentRequest, Department>();
        CreateMap<Department, DepartmentResponse>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.CompanyName, opt => opt.MapFrom(x => x.Company.Name))
            .ForMember(x=>x.UnitDepartments, 
                opt=>opt.MapFrom<UnitDepartmentResolver>());
        
        CreateMap<List<Department>, List<DepartmentResponse>>()
            .ConvertUsing<DepartmentConverter>();

        CreateMap<DepartmentImportModel, Department>()
            .ReverseMap();
    }
}

public sealed class UnitDepartmentResolver : IValueResolver<Department, DepartmentResponse, List<UnitDepartmentResponse>>
{
    public List<UnitDepartmentResponse> Resolve(Department source, DepartmentResponse destination, List<UnitDepartmentResponse> destMember, ResolutionContext context)
    {
        destination.UnitDepartments = new List<UnitDepartmentResponse>();
        foreach (var unitDepartment in source.UnitDepartments)
        {
            var unitDepartmentResponse = new UnitDepartmentResponse
            {
                Id = unitDepartment.Id,
                Name = unitDepartment.Name,
                DepartmentName = source.Name,
                CompanyName = source.Company.Name,
                OrdinalNumber = source.UnitDepartments.IndexOf(unitDepartment) + 1
            };
            destination.UnitDepartments.Add(unitDepartmentResponse);
        }
        return destination.UnitDepartments;
    }
}

public sealed class DepartmentConverter : ITypeConverter<List<Department>, List<DepartmentResponse>>
{
    private readonly IMapper _mapper;

    public DepartmentConverter(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public List<DepartmentResponse> Convert(List<Department> source, List<DepartmentResponse> destination, ResolutionContext context)
    {
        destination = new List<DepartmentResponse>();

        for (int i = 0; i < source.Count; i++)
        {
            var departmentResponse = _mapper.Map<DepartmentResponse>(source[i]);
            departmentResponse.OrdinalNumber = i + 1;
            destination.Add(departmentResponse);
        }

        return destination;
    }
}
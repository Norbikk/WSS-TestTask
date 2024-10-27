using AutoMapper;
using TaskWSS.Models;
using TaskWSS.ViewModels;
using TaskWSS.ViewModels.ImportModels;
using TaskWSS.ViewModels.Response;

namespace TaskWSS.AutoMapper;

public class UnitDepartmentProfile : Profile
{
    public UnitDepartmentProfile()
    {
        CreateMap<UnitDepartment, UnitDepartmentResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Department.Company.Name));
        
        CreateMap<List<UnitDepartment>, List<UnitDepartmentResponse>>()
            .ConvertUsing<UnitDepartmentConverter>();

        CreateMap<CreateUnitDepartmentRequest, UnitDepartment>();
        CreateMap<UpdateUnitDepartmentRequest, UnitDepartment>();

        CreateMap<UnitDepartmentImportModel, UnitDepartment>()
            .ReverseMap();
    }
}

public sealed class UnitDepartmentConverter : ITypeConverter<List<UnitDepartment>, List<UnitDepartmentResponse>>
{
    private readonly IMapper _mapper;

    public UnitDepartmentConverter(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public List<UnitDepartmentResponse> Convert(List<UnitDepartment> source, List<UnitDepartmentResponse> destination, ResolutionContext context)
    {
        destination = new List<UnitDepartmentResponse>();

        for (int i = 0; i < source.Count; i++)
        {
            var unitDepartmentResponse = _mapper.Map<UnitDepartmentResponse>(source[i]);
            unitDepartmentResponse.OrdinalNumber = i + 1;
            destination.Add(unitDepartmentResponse);
        }

        return destination;
    }
}
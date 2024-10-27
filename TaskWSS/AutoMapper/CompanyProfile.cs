using AutoMapper;
using TaskWSS.Models;
using TaskWSS.ViewModels;
using TaskWSS.ViewModels.ImportModels;
using TaskWSS.ViewModels.Response;

namespace TaskWSS.AutoMapper;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<Company, CompanyResponse>()
            .ForMember(x=>x.Departments, 
                opt=>opt.MapFrom<DepartmentsResolver>());
        
        CreateMap<List<Company>, List<CompanyResponse>>()
            .ConvertUsing<CompaniesConverter>();
        

        CreateMap<CreateCompanyRequest, Company>();
        CreateMap<UpdateCompanyRequest, Company>();

        CreateMap<CompanyImportModel, Company>()
            .ReverseMap();
    }   
}
public sealed class DepartmentsResolver : IValueResolver<Company, CompanyResponse, List<DepartmentResponse>>
{
    private readonly IMapper _mapper;
    
    public DepartmentsResolver(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public List<DepartmentResponse> Resolve(Company source, CompanyResponse destination, List<DepartmentResponse> destMember, ResolutionContext context)
    {
        destination.Departments = new List<DepartmentResponse>();
        
        foreach (var departmentResponse in source.Departments.Select(department => new DepartmentResponse
                 {
                     Id = department.Id,
                     CompanyName = source.Name,
                     Name = department.Name,
                     OrdinalNumber = source.Departments.IndexOf(department) + 1,
                     UnitDepartments = _mapper.Map<List<UnitDepartmentResponse>>(department.UnitDepartments)
                 }))
        {
            for (int i = 0; i < departmentResponse.UnitDepartments.Count; i++)
            {
                departmentResponse.UnitDepartments[i].OrdinalNumber = i + 1;
            }
            
            destination.Departments.Add(departmentResponse);
        }
        return  destination.Departments;
    }
}
public sealed class CompaniesConverter : ITypeConverter<List<Company>, List<CompanyResponse>>
{
    private readonly IMapper _mapper;

    public CompaniesConverter(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public List<CompanyResponse> Convert(List<Company> source, List<CompanyResponse> destination, ResolutionContext context)
    {
        destination = new List<CompanyResponse>();

        for (int i = 0; i < source.Count; i++)
        {
            var companyResponse = _mapper.Map<CompanyResponse>(source[i]);
            companyResponse.OrdinalNumber = i + 1;
            destination.Add(companyResponse);
        }

        return destination;
    }
}
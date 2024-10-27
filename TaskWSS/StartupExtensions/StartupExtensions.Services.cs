using AutoMapper;
using Microsoft.OpenApi.Models;
using TaskWSS.DatabaseContext;
using Swashbuckle.AspNetCore.SwaggerGen;
using TaskWSS.Filter;
using TaskWSS.Interfaces;
using TaskWSS.Services;
using TaskWSS.Services.XmlService;

namespace TaskWSS.StartupExtensions;

public static partial class StartupExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<DbInitializer>();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v0", new OpenApiInfo { Title = "TaskVSS API", Version = "v0" });
            c.OperationFilter<FileUploadOperationFilter>();
        });
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IUnitDepartmentService, UnitDepartmentService>();
        services.AddScoped<ICompanyImportService, ImportXmlService>();
        services.AddScoped<ICompanyExportService, ExportXmlService>();
    }
}
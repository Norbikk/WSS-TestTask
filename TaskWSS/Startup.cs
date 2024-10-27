using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TaskWSS.DatabaseContext;
using TaskWSS.StartupExtensions;

namespace TaskWSS;

public class Startup
{
    public IWebHostEnvironment WebHostEnvironment { get; }
    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        WebHostEnvironment = webHostEnvironment;
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        string? connectionString = Configuration.GetConnectionString("PostgreSqlConnection");
        
        services.AddHttpContextAccessor();
        

        services.AddDbContextPool<TaskDatabaseContext>(options => options.UseNpgsql(connectionString));
        
        services.AddControllers();
        
        services.AddServices();
        
        services.AddResponseCompression();
    }


    public void Configure(IApplicationBuilder app,
        IWebHostEnvironment env,
        DbInitializer dbInitializer,
        TaskDatabaseContext dbContext)
    {
        dbContext.Database.EnsureCreated();
        
        dbInitializer.Initialize().Wait();
        dbInitializer.Dispose();
        
        
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());
        }
        else
        {
            app.UseHsts();
        }
        
        
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                var statusCode = GetErrorResponseStatusCode(exception);
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    error = exception.Message
                }));
            });
        });

        
        app.UseForwardedHeaders();
        
        app.UseSwagger();
        
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v0/swagger.json", "v0");
            options.RoutePrefix = String.Empty;
            options.InjectJavascript("/swagger-ui/custom.js");
            options.InjectJavascript("/swagger-ui/docs/rapipdf-min.js");
        });
        
        app.UseResponseCompression();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
    
    private static int GetErrorResponseStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }

}
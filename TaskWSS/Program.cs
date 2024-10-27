using TaskWSS;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

internal class Program
{
    public static void Main(string[] args)
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
    
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration(cfg =>
            {
#if DEBUG
                cfg.AddJsonFile("appsettings.Development.json");
                cfg.AddUserSecrets<Program>();
#else
                cfg.AddJsonFile("appsettings.json");
#endif

                cfg.AddEnvironmentVariables();
            })
            .ConfigureLogging((context, logging) =>
            {
#if !DEBUG
                    logging.ClearProviders();
#endif

                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .UseNLog();
}
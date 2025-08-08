using Serilog.Formatting.Json;
using Serilog;


namespace FGC.Api.Extensions
{
    
    public static class SerilogExtensions
    {
        public static void ConfigureSerilog(HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration)
        {
            configuration
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "FGC.Api")
                .WriteTo.Console()
                .WriteTo.File(
                    new JsonFormatter(renderMessage: true),
                    "logs/log-.json",
                    rollingInterval: RollingInterval.Day);
        }
    }
}

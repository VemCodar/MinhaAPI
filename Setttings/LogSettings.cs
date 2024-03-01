using System.Reflection;
using Serilog;

namespace MinhaAPI.Setttings;

public static class LogSettings
{
    public static void AddLogSettings(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .Enrich.WithProperty("Version", Assembly.GetEntryAssembly()!.GetName().Version)
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithMemoryUsage()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341/")
            .CreateLogger();
        builder.Logging.AddSerilog(logger);
    }
}
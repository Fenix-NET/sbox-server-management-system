using Microsoft.Extensions.Configuration;

using Serilog;
using Serilog.Core;

namespace Infrastructure.Logging;

public class SerilogConfiguration
{
    public static Logger CreateLogger(IConfiguration configuration)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.FromLogContext()
            .WriteTo.Console();

        ELKLogging.ConfigureElkLogging(loggerConfiguration, configuration);

        return loggerConfiguration.CreateLogger();
    }
}
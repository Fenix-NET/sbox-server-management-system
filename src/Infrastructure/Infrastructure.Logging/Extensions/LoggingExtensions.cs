using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure.Logging.Extensions;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddSerilogLogging(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var logger = SerilogConfiguration.CreateLogger(configuration);
        builder.AddSerilog(logger);
        return builder;
    }
}
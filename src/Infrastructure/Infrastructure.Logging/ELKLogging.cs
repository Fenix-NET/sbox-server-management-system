using Serilog.Sinks.Elasticsearch;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Logging
{
    public static class ELKLogging
    {
        public static void ConfigureElkLogging(LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            var elasticUri = configuration["ElasticConfiguration:Uri"];

            if (!string.IsNullOrEmpty(elasticUri))
            {
                loggerConfiguration
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{configuration["ApplicationName"]}-logs-{DateTime.UtcNow:yyyy-MM}",
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                })
                .Enrich.WithProperty("Environment", configuration["Environment"])
                .ReadFrom.Configuration(configuration);
            }
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace Infrastructure.Monitoring
{
    public class MonitoringService
    {
        public void ConfigureMonitoring(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics(); // Интеграция с Prometheus
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}

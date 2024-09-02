using Infrastructure.Monitoring.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Monitoring.Extensions
{
    public static class MonitoringExtensions
    {
        /// <summary>
        /// Добавление всех служб Мониторинга
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonitoring(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                    .AddCheck<HealthCheckService>("default_health_check");

            services.AddPrometheusMetrics();
            return services;
        }
        /// <summary>
        /// Конфигурация метрик Prometheus
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPrometheusMetrics(this IServiceCollection services)
        {
            return services;
        }
        /// <summary>
        /// Добавление сервера метрик Prometheus
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePrometheusMetrics(this IApplicationBuilder app)
        {
            app.UseMetricServer();
            return app;
        }

    }
}

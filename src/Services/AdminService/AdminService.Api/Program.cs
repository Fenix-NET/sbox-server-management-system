
using Microsoft.Extensions.Configuration;
using Infrastructure.Logging.Extensions;
using Infrastructure.Monitoring.Extensions;
using Infrastructure.Messaging.EventBus;
using Infrastructure.Messaging.Extensions;

namespace AdminService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = builder.Configuration;
            
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilogLogging(config);
            });

            builder.Services.AddMonitoring(config);

            builder.Services.AddEventBus(config);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UsePrometheusMetrics();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            var eventBus = app.Services.GetRequiredService<IEventBus>();

            app.Run();
        }
    }
}

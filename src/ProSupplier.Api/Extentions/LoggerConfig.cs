using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace ProSupplier.Api.Extentions
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "de578cdbdcb342fab73d94c4e4c09795";
                o.LogId = new Guid("584f09ae-fe17-486f-9a50-8acac450a90d");
            });

            //services.AddLogging(builder =>
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "de578cdbdcb342fab73d94c4e4c09795";
            //        o.LogId = new Guid("584f09ae-fe17-486f-9a50-8acac450a90d");
            //    });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);

            //});

            // Health-Checks Configuration
            services.AddHealthChecks()
                          .AddElmahIoPublisher(options =>
                          {
                              options.ApiKey = "de578cdbdcb342fab73d94c4e4c09795";
                              options.LogId = new Guid("584f09ae-fe17-486f-9a50-8acac450a90d");
                              options.HeartbeatId = "API Suppliers";
                          })
                .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            services.AddHealthChecksUI()
                        .AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));


            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}

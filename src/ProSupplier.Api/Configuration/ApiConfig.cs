using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ProSupplier.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder =>
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

                options.AddPolicy("Production",
                    builder =>
                       builder
                       .WithMethods("GET")
                       .WithOrigins("*")
                       .SetIsOriginAllowedToAllowWildcardSubdomains()
                       //.WithHeaders(HeaderNames.ContentType,  "x-custom-header")
                       .AllowAnyHeader());

            });

            // Sobre versionamentos 
            services.AddApiVersioning(options => 
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            // Configuração do AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        public static IApplicationBuilder UseMvcConfiguration(this IApplicationBuilder app)
        {
            // Força redirect para HTTPS
            app.UseHttpsRedirection();

            return app;
        }
    }
}

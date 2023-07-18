using Microsoft.Extensions.Options;
using ProSupplier.Api.Extentions;
using ProSupplier.Business.Interfaces;
using ProSupplier.Business.Notifications;
using ProSupplier.Business.Services;
using ProSupplier.Data.Context;
using ProSupplier.Data.Repository;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProSupplier.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MyDbContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IProdutoService, ProdutoService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}

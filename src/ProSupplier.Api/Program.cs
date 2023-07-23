using ProSupplier.Api.Configuration;
using ProSupplier.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ProSupplier.Api.Extentions;


var builder = WebApplication.CreateBuilder(args);

// Adicionando suporte a Diversos arquivos de configuração por ambiente.
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
.AddEnvironmentVariables();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Adicionando suporte a User Secrets
if (builder.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// *** Configurando serviços no container ***

builder.Services.WebApiConfig();

// Swagger Configurations
builder.Services.AddSwaggerConfig();
builder.Services.AddSwaggerGen();

// Extension Method de resolução de DI
builder.Services.ResolveDependencies();

// Adicionando suporte a componentes Razor (ex: Telas do Identity)
builder.Services.AddRazorPages();

// Extension Method de configuração do Identity
builder.Services.AddIdentityConfiguration(builder.Configuration);

// Extension Method de Authorization (Policies)
//builder.Services.AddAuthorizationConfig();

builder.Services.AddLoggingConfiguration(builder.Configuration);

// Extension Method de configuração KissLog
//builder.Services.RegisterKissLogListeners();

// Adicionando o MVC com suporte ao filtro de auditoria
//builder.Services.AddControllersWithViews(options =>
//{
//    options.Filters.Add(typeof(AuditoriaFilter));
//});



var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// *** Configurando o resquest dos serviços no pipeline ***

app.UseSwagger();
app.UseSwaggerUI();


if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseCors("Production");
    app.UseExceptionHandler("/erro/500");
    app.UseStatusCodePagesWithRedirects("/erro/{0}");
    app.UseHsts();
}


// Adicionando suporte a arquivos (ex: CSS, JS)
app.UseStaticFiles();

// Adicionando suporte a rota
app.UseRouting();

// Autenticacao e autorização (Identity)
app.UseAuthentication();
app.UseAuthorization();

// Rota padrão
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Mapeando componentes Razor Pages (ex: Identity)
app.MapRazorPages();

app.UseLoggingConfiguration();

app.UseMiddleware<ExceptionMiddleware>();

app.UseMvcConfiguration();

app.UseSwaggerConfig(apiVersionDescriptionProvider);

app.Run();
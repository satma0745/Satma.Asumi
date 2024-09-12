using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Satma.Asumi.Web.Components;
using Satma.Asumi.Web.Persistence;
using Satma.Asumi.Web.Services;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

var postgresConnectionString = webApplicationBuilder.Configuration.GetConnectionString("PostgreSQL");
webApplicationBuilder.Services.AddDbContext<AsumiDbContext>(options => options.UseNpgsql(postgresConnectionString));

webApplicationBuilder.Services.AddSwaggerGen();
webApplicationBuilder.Services.AddControllers();

webApplicationBuilder.Services.AddSingleton<JwtSigningKeyService>();
webApplicationBuilder.Services.AddSingleton<JwtTokenService>();

webApplicationBuilder.Services.AddMudServices();
webApplicationBuilder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var webApplication = webApplicationBuilder.Build();

if (!webApplication.Environment.IsDevelopment())
{
    webApplication.UseExceptionHandler("/Error", createScopeForErrors: true);
}

if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}

webApplication.UseStaticFiles();
webApplication.UseAntiforgery();

webApplication.MapControllers();

webApplication
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

webApplication.Run();

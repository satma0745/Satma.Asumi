using Satma.Asumi.Web.Components;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

webApplicationBuilder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var webApplication = webApplicationBuilder.Build();

if (!webApplication.Environment.IsDevelopment())
{
    webApplication.UseExceptionHandler("/Error", createScopeForErrors: true);
}

webApplication.UseStaticFiles();
webApplication.UseAntiforgery();

webApplication
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

webApplication.Run();

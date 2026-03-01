using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Diagnostics.Injection.Web;
using Rocket.Dropbox.Injection.Web;
using Rocket.Gcp.Injection.Web;
using Rocket.Google.Injection.Web;
using Rocket.Microsofts.Injection.Web;
using Rocket.Notion.Injection.Web;
using Rocket.Ollama.Injection.Web;
using Rocket.QuestPdf.Injection.Web;
using Rocket.Replicate.Injection.Web;
using Rocket.Web.Host.Components;
using Rocket.Web.Host.Injection;

var builder = 
    WebApplication
        .CreateBuilder(args);

var services = 
    builder
        .Services;

var configuration =
    builder
        .Configuration;

var environment =
    builder
        .Environment;

// Add services to the container.
services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

services
    .AddCascadingAuthenticationState();

services
    .AddAuthentication(
        options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }
    )
    .AddIdentityCookies();

services
    .AddBottleRocketWebServices(configuration, environment)
    .AddMudBlazorServices(configuration);

services
    .AddServerSideBlazor();

services
    .AddAuthorizationCore()
    .AddAuthenticationServices();

services
    .AddDiagnosticWebIntegration()
    .AddDropboxWebIntegration()
    .AddGcpWebIntegration()
    .AddGoogleWebIntegration()
    .AddMicrosoftWebIntegration()
    .AddNotionWebIntegration()
    .AddOllamaWebIntegration()
    .AddQuestPdfWebIntegration()
    .AddReplicateWebIntegration();

services
    .AddSignalRClientServices();

services
    .AddJsonSupport();

var app = 
    builder
        .Build();


app
    .UseStatusCodePagesWithReExecute("/Error");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
else
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true
    );
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseAntiforgery();

app
    .MapStaticAssets();

app
    .MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app
    .Run();
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
// services.AddScoped<IdentityUserAccessor>();
// services.AddScoped<IdentityRedirectManager>();
// services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

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

var app = 
    builder
        .Build();


app
    .UseStatusCodePagesWithReExecute("/NotFound");

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
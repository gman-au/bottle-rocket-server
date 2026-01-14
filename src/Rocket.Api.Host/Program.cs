using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Host;
using Rocket.Api.Host.Filters;
using Rocket.Api.Host.Handlers;
using Rocket.Api.Host.Hubs;
using Rocket.Api.Host.Injection;
using Rocket.Domain.Utils;
using Rocket.Dropbox.Injection;

var builder =
    WebApplication
        .CreateBuilder(args);

var configuration =
    builder
        .Configuration;

var services =
    builder
        .Services;

services
    .AddControllers();

services
    .AddMvc(options => options.Filters.Add<RocketExceptionFilter>());

services
    .AddMongoDbServices(configuration)
    .AddLocalFileBlobStore(configuration)
    .AddBottleRocketApiServices(builder.Environment);

services
    .AddEndpointsApiExplorer();

services
    .AddAuthentication(DomainConstants.BasicAuthentication)
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
        DomainConstants.BasicAuthentication,
        null
    );

services
    .AddAuthorization();

services
    .AddSignalRServerServices(configuration);

services
    .AddOpenApiServices();

services
    .AddDropboxIntegration();

services
    .AddHostedService<StartupInitializationHostedService>();

var app =
    builder
        .Build();

app
    .UseCors(DomainConstants.BlazorAppCorsPolicy);

// Add authentication middleware
app
    .UseAuthentication()
    .UseAuthorization();

app
    .UseHttpsRedirection();

app
    .MapOpenApi(); // Available at /openapi/v1.json by default

app
    .MapControllers();

app
    .MapHub<CaptureHub>("/hubs/capture");

app.Run();
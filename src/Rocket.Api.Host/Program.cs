using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Api.Host;
using Rocket.Api.Host.Filters;
using Rocket.Api.Host.Handlers;
using Rocket.Api.Host.Hubs;
using Rocket.Api.Host.Injection;
using Rocket.Domain.Utils;
using Rocket.Dropbox.Injection;
using Rocket.Infrastructure.Json;

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
    .Configure<HostOptions>(
        options =>
        {
            options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        }
    );

services
    .AddControllers()
    .AddJsonOptions(
        options =>
            options.JsonSerializerOptions.TypeInfoResolver = RocketTypeInfoResolver.Instance
    );

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
    .AddDropboxIntegration()
    .AddWorkflowBackgroundJob();

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
using System.Threading;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Host.Filters;
using Rocket.Api.Host.Handlers;
using Rocket.Api.Host.Hubs;
using Rocket.Api.Host.Injection;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

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

var app =
    builder
        .Build();

app
    .UseCors(DomainConstants.BlazorAppCorsPolicy);

// Run startup initialization
using (var scope = app.Services.CreateScope())
{
    var initService =
        scope
            .ServiceProvider
            .GetRequiredService<IStartupInitialization>();

    await
        initService
            .InitializeAsync(CancellationToken.None);
}

// Add authentication middleware
app
    .UseAuthentication()
    .UseAuthorization();

app
    .UseHttpsRedirection();

app
    .MapControllers();

app
    .MapHub<CaptureHub>("/hubs/capture");

app.Run();
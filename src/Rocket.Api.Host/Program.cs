using System.Threading;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Host.Filters;
using Rocket.Api.Host.Handlers;
using Rocket.Api.Host.Injection;
using Rocket.Interfaces;

const string basicAuth = "BasicAuthentication";

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
    .AddAuthentication(basicAuth)
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
        basicAuth,
        null
    );

services
    .AddAuthorization();

var app =
    builder
        .Build();

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

app.Run();
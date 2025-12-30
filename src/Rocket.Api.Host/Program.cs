using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Host.Filters;
using Rocket.Api.Host.Handlers;
using Rocket.Api.Host.Injection;

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
    .AddBottleRocketApiServices();

services
    .AddEndpointsApiExplorer();

services
    .AddAuthentication(basicAuth)
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(basicAuth, null);

services
    .AddAuthorization();

var app =
    builder
        .Build();

// Add authentication middleware
app
    .UseAuthentication()
    .UseAuthorization();

app
    .UseHttpsRedirection();

app
    .MapControllers();

app.Run();
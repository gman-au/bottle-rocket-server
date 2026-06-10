using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Host.Filters;
using Rocket.Api.Host.Handlers;
using Rocket.Api.Host.OpenApi.Injection;
using Rocket.Domain.Utils;

var builder =
    WebApplication
        .CreateBuilder(args);

var services =
    builder
        .Services;

services
    .AddControllers();

services
    .AddMvc(options => options.Filters.Add<RocketExceptionFilter>());

services
    .AddEndpointsApiExplorer();

services
    .AddAuthentication(DomainConstants.BasicAuthentication)
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
        DomainConstants.BasicAuthentication,
        null
    );

services
    .AddOpenApiServices();

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
    .MapOpenApi(); // Available at /openapi/v1.json by default

app
    .MapControllers();


app.Run();
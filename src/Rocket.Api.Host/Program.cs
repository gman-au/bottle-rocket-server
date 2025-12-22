using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Host.Filters;
using Rocket.Api.Host.Injection;

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

var app =
    builder
        .Build();

app
    .UseHttpsRedirection()
    .UseAuthorization();

app
    .MapControllers();

app.Run();
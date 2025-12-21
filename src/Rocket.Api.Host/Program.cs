using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
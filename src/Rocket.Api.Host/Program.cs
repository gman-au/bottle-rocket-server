using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Api.Host;
using Rocket.Api.Host.Filters;
using Rocket.Api.Host.Handlers;
using Rocket.Api.Host.Hubs;
using Rocket.Api.Host.Injection;
using Rocket.Diagnostics.Injection.Api;
using Rocket.Domain.Utils;
using Rocket.Dropbox.Injection.Api;
using Rocket.Gcp.Injection.Api;
using Rocket.Google.Injection.Api;
using Rocket.Interfaces;
using Rocket.Microsofts.Injection.Api;
using Rocket.Notion.Injection.Api;
using Rocket.Ollama.Injection.Api;
using Rocket.QuestPdf.Injection.Api;
using Rocket.Replicate.Injection.Api;

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
        options => { options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore; }
    );

services
    .AddControllers();

services
    .AddOptions<Microsoft.AspNetCore.Mvc.JsonOptions>()
    .Configure<IJsonResolverInstanceProvider>(
        (jsonOptions, resolverProvider) => { jsonOptions.JsonSerializerOptions.TypeInfoResolver = resolverProvider.GetInstance(); }
    );
/*.AddJsonOptions(
    options =>
        options.JsonSerializerOptions.TypeInfoResolver = RocketTypeInfoResolver.Instance
);*/

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
    .AddDropboxApiIntegration()
    .AddOllamaApiIntegration()
    .AddNotionApiIntegration()
    .AddMicrosoftApiIntegration()
    .AddGcpApiIntegration()
    .AddGoogleApiIntegration()
    .AddDiagnosticApiIntegration()
    .AddQuestPdfApiIntegration()
    .AddReplicateApiIntegration();

services
    .AddWorkflowBackgroundJob();

services
    .AddHostedService<StartupInitializationHostedService>();

// call this after all of the integrations
services
    .RegisterBsonDomainMappings();

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
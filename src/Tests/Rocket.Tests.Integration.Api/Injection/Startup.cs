using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using Rocket.Tests.Infrastructure;
using Rocket.Tests.Infrastructure.Contexts;
using Rocket.Tests.Integration.Api.Engine;

namespace Rocket.Tests.Integration.Api.Injection
{
    public class Startup
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services =
                Achar
                    .Infrastructure
                    .ReqnRoll
                    .Injection
                    .Startup
                    .CreateServices();

            services
                .AddSingleton<IContainerOrchestrator, ContainerOrchestrator>();

            services
                .AddSingleton<IServiceContext, TestContainerServiceContext>();

            services
                .AddSingleton<IApiExtendedInteractionEngine, ApiExtendedHttpClientInteractionEngine>();

            return services;
        }
    }
}
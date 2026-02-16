using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using Rocket.Tests.Infrastructure;
using Rocket.Tests.Infrastructure.Contexts;

namespace Rocket.Tests.Integration.Api.Injection
{
    public class Startup
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            services
                .AddSingleton<IContainerOrchestrator, ContainerOrchestrator>();

            services
                .AddSingleton<IServiceContext, TestContainerServiceContext>();

            return services;
        }
    }
}
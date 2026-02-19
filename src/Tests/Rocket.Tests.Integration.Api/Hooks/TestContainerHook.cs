using System.Threading.Tasks;
using Reqnroll;
using Rocket.Tests.Infrastructure.Contexts;

namespace Rocket.Tests.Integration.Api.Hooks
{
    [Binding]
    public class TestContainerHook
    {
        [BeforeScenario]
        public static async Task SetupTestEnvironment(IServiceContext context)
        {
            await
                context
                    .SetupEnvironmentContextAsync();
        }

        [AfterScenario]
        public static async Task TeardownTestEnvironment(IServiceContext context)
        {
            await
                context
                    .TeardownEnvironmentContextAsync();
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Reqnroll;
using Rocket.Tests.Infrastructure.Contexts;
using Rocket.Tests.Infrastructure.Definition;

namespace Rocket.Tests.Integration.Api.Hooks
{
    [Binding]
    public class TestContainerHook
    {
        private const string ShortJwtExpiryTag = "SHORT-JWT-EXPIRY";
        
        private readonly ScenarioContext _scenarioContext;

        public TestContainerHook(
            ScenarioContext scenarioContext
        )
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task SetupTestEnvironment(
            IServiceContext context
        )
        {
            var tags =
                _scenarioContext
                    .ScenarioInfo
                    .CombinedTags
                    .ToList();

            var orchestrationParameters =
                new OrchestrationParameters
                {
                    DatabaseName = "BottleRocket"
                };

            if (tags.Contains(ShortJwtExpiryTag))
                orchestrationParameters.JwtExpirySeconds = 20;

            await
                context
                    .SetupEnvironmentContextAsync(orchestrationParameters);
        }

        [AfterScenario]
        public async Task TeardownTestEnvironment(
            IServiceContext context
        )
        {
            await
                context
                    .TeardownEnvironmentContextAsync();
        }
    }
}
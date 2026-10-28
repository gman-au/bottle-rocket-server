using System.Threading.Tasks;
using MongoDB.Driver;
using Rocket.Tests.Infrastructure.Definition;
using Testcontainers.MongoDb;

namespace Rocket.Tests.Infrastructure.Contexts
{
    public class TestContainerServiceContext(
        IContainerOrchestrator orchestrator
    ) : IServiceContext
    {
        private MongoDbContainer _mongoDbContainer;
        private OrchestrationParameters _orchestrationParameters;

        public async Task SetupEnvironmentContextAsync(OrchestrationParameters parameters = null)
        {
            _orchestrationParameters = parameters;
            
            await
                orchestrator
                    .ArrangeDockerNetwork();

            _mongoDbContainer =
                await
                    orchestrator
                        .ArrangeDatabaseContainerAsync();

            await
                orchestrator
                    .ArrangeApiContainerAsync(_orchestrationParameters);
        }

        public async Task TeardownEnvironmentContextAsync()
        {
            await
                Task
                    .WhenAll(
                        orchestrator
                            .TeardownAppContainersAsync(),
                        orchestrator
                            .TeardownDatabaseContainerAsync()
                    );

            await
                orchestrator
                    .TeardownDockerNetwork();
        }

        public async Task ResetDatabaseAsync()
        {
            await
                orchestrator
                    .ArrangeDatabaseResetAsync();
        }

        public async Task<T> RunMongoCommandAsync<T>(
            Command<T> command
        )
        {
            var connectionString =
                _mongoDbContainer
                    .GetConnectionString();

            var mongoClient =
                new MongoClient(connectionString);

            var mongoDatabase =
                mongoClient
                    .GetDatabase(_orchestrationParameters?.DatabaseName ?? "BottleRocket");

            return
                await
                    mongoDatabase
                        .RunCommandAsync(command);
        }
    }
}
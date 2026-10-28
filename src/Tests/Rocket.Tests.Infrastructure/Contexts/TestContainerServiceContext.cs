using System.Threading.Tasks;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace Rocket.Tests.Infrastructure.Contexts
{
    public class TestContainerServiceContext(
        IContainerOrchestrator orchestrator
    ) : IServiceContext
    {
        private const string DatabaseName = "BottleRocket";

        private MongoDbContainer _mongoDbContainer;

        public async Task SetupEnvironmentContextAsync()
        {
            await
                orchestrator
                    .ArrangeDockerNetwork();

            _mongoDbContainer =
                await
                    orchestrator
                        .ArrangeDatabaseContainerAsync();

            await
                orchestrator
                    .ArrangeApiContainerAsync(DatabaseName);
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
                    .GetDatabase(DatabaseName);

            return
                await
                    mongoDatabase
                        .RunCommandAsync(command);
        }
    }
}
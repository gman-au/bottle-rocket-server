using System;
using System.Threading.Tasks;
using Testcontainers.MongoDb;

namespace Rocket.Tests.Infrastructure.Contexts
{
    public class TestContainerServiceContext(IContainerOrchestrator orchestrator) : IServiceContext
    {
        private Uri _apiUri;
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

            var apiContainer =
                await
                    orchestrator
                        .ArrangeApiContainerAsync();

            _apiUri =
                new UriBuilder(
                    "http",
                    apiContainer.Hostname,
                    apiContainer.GetMappedPublicPort(8080)
                ).Uri;
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

        public async Task RunSqlCommandsAsync(params string[] sqlCommands)
        {

            /*var connection = new SqlConnection(_mongoDbContainer.GetConnectionString());

            await
                connection
                    .OpenAsync();

            foreach (var sqlCommand in sqlCommands)
            {
                await
                    using var command =
                        connection
                            .CreateCommand();

                command.CommandText = sqlCommand;

                await
                    command
                        .ExecuteNonQueryAsync();
            }*/
        }
    }
}
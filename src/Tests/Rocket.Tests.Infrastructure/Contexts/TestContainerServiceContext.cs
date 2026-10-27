using System;
using System.Threading.Tasks;
using Testcontainers.MongoDb;

namespace Rocket.Tests.Infrastructure.Contexts
{
    public class TestContainerServiceContext(
        IContainerOrchestrator orchestrator
    ) : IServiceContext
    {
        public async Task SetupEnvironmentContextAsync()
        {
            await
                orchestrator
                    .ArrangeDockerNetwork();

            await
                orchestrator
                    .ArrangeDatabaseContainerAsync();

            await
                orchestrator
                    .ArrangeApiContainerAsync();
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

        public async Task RunSqlCommandsAsync(
            params string[] sqlCommands
        )
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
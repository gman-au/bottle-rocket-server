using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Testcontainers.MongoDb;

namespace Rocket.Tests.Infrastructure
{
    public interface IContainerOrchestrator
    {
        Task<INetwork> ArrangeDockerNetwork();

        Task TeardownDockerNetwork();

        Task ArrangeDatabaseResetAsync();

        Task<MongoDbContainer> ArrangeDatabaseContainerAsync();

        Task TeardownDatabaseContainerAsync();

        Task<IContainer> ArrangeApiContainerAsync();

        Task TeardownAppContainersAsync();
    }
}
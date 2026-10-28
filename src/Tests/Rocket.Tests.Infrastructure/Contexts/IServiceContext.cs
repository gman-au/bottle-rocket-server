using System.Threading.Tasks;
using MongoDB.Driver;
using Rocket.Tests.Infrastructure.Definition;

namespace Rocket.Tests.Infrastructure.Contexts
{
    public interface IServiceContext
    {
        Task SetupEnvironmentContextAsync(OrchestrationParameters parameters = null);

        Task TeardownEnvironmentContextAsync();

        Task ResetDatabaseAsync();

        Task<T> RunMongoCommandAsync<T>(
            Command<T> command
        );
    }
}
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Rocket.Tests.Infrastructure.Contexts
{
    public interface IServiceContext
    {
        Task SetupEnvironmentContextAsync();

        Task TeardownEnvironmentContextAsync();

        Task ResetDatabaseAsync();

        Task<T> RunMongoCommandAsync<T>(
            Command<T> command
        );
    }
}
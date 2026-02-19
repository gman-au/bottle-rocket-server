using System.Threading.Tasks;

namespace Rocket.Tests.Infrastructure.Contexts
{
    public interface IServiceContext
    {
        Task SetupEnvironmentContextAsync();

        Task TeardownEnvironmentContextAsync();

        Task ResetDatabaseAsync();

        Task RunSqlCommandsAsync(params string[] sqlCommands);
    }
}
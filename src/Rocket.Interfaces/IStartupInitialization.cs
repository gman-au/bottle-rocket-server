using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;

namespace Rocket.Interfaces
{
    public interface IStartupInitialization
    {
        Task InitializeAsync(CancellationToken cancellationToken);
        
        Task<StartupPhaseEnum> GetStartupPhaseAsync(CancellationToken cancellationToken);
    }
}
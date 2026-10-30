using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestSystem
    {
        Task<StartupPhaseResponse> GetStartupPhaseAsync(
            CancellationToken cancellationToken
        );
        
        Task<VersionResponse> GetSystemVersionsAsync(
            CancellationToken cancellationToken
        );
    }
}
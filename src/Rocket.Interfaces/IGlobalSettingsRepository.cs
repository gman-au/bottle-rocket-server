using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IGlobalSettingsRepository
    {
        Task UpdateGlobalSettingsAsync(CancellationToken cancellationToken);

        Task<GlobalSettings> GetGlobalSettingsAsync(CancellationToken cancellationToken);
    }
}
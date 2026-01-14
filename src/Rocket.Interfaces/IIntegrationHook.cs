using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IIntegrationHook
    {
        Task ProcessAsync(
            string userId,
            byte[] fileData,
            string fileExtension,
            CancellationToken cancellationToken
        );
    }
}
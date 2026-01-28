using System.Threading;
using System.Threading.Tasks;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public interface IMicrosoftTokenAcquirer
    {
        Task<MicrosoftDeviceCodeResult> AcquireAccountIdentifierAsync(
            MicrosoftConnector connector,
            string userId,
            CancellationToken cancellationToken
        );

        Task<string> AcquireTokenSilentAsync(
            MicrosoftConnector connector,
            CancellationToken cancellationToken
        );
    }
}
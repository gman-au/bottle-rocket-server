using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IDatabasePrepopulator
    {
        Task PopulatePageTemplatesAsync(CancellationToken cancellationToken);
    }
}
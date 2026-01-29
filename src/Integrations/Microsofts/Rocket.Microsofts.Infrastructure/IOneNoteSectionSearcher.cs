using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public interface IOneNoteSectionSearcher
    {
        Task<IEnumerable<OneNoteSection>> GetSectionsAsync(
            MicrosoftConnector connector,
            CancellationToken cancellationToken
        );
    }
}
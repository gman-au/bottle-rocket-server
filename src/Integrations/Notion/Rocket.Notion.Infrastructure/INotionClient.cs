using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Notion.Contracts;

namespace Rocket.Notion.Infrastructure
{
    public interface INotionClient
    {
        Task<IEnumerable<NotionParentNoteSummary>> GetParentNotesAsync(
            string integrationSecret,
            CancellationToken cancellationToken
        );
    }
}
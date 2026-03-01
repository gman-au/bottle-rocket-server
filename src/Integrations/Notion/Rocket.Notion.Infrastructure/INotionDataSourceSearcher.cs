using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Notion.Contracts;

namespace Rocket.Notion.Infrastructure
{
    public interface INotionDataSourceSearcher
    {
        Task<IEnumerable<NotionDataSourceSummary>> GetDataSourcesAsync(
            string integrationSecret,
            CancellationToken cancellationToken
        );
    }
}
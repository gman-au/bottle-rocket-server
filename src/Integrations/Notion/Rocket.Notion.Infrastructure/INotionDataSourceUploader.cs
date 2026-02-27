using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Notion.Infrastructure
{
    public interface INotionDataSourceUploader
    {
        Task UploadDataSourceAsync(
            string integrationSecret,
            string dataSourceId,
            IEnumerable<Tuple<string, string>> dataSourcePairs,
            CancellationToken cancellationToken
        );
    }
}
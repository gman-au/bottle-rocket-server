using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Markdig.Infrastructure.Definition;

namespace Rocket.Markdig.Infrastructure
{
    public interface IEpubRenderer
    {
        Task<byte[]> RenderToStreamAsync(
            Metadata metadata,
            IReadOnlyList<Chapter> chapters,
            string lang,
            CancellationToken cancellationToken = default
        );
    }
}
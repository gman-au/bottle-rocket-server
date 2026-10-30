using System.Threading;
using System.Threading.Tasks;
using Rocket.Notion.Contracts;

namespace Rocket.Web.Infrastructure.Client.Vendors
{
    public interface IApiRequestNotion
    {
        Task<GetAllNotionParentNotesResponse> GetNotionParentNotesAsync(
            GetAllNotionParentNotesRequest request,
            CancellationToken cancellationToken
        );

        Task<GetAllNotionDataSourcesResponse> GetNotionDataSourcesAsync(
            GetAllNotionDataSourcesRequest request,
            CancellationToken cancellationToken
        );
    }
}
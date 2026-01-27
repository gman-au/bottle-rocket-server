using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Notion.Contracts;
using Rocket.Web.Client.Extensions;

namespace Rocket.Web.Client
{
    public partial class ApiRequestManager
    {
        public async Task<GetAllNotionParentNotesResponse> GetNotionParentNotesAsync(
            GetAllNotionParentNotesRequest request,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received Get (Notion) parent notes request");

            var response =
                await
                    authenticatedApiClient
                        .PostAsJsonAsync(
                            "/api/notion/workflows/getParentNotes",
                            request,
                            cancellationToken
                        );

            var result =
                await
                    response
                        .TryParseResponse<GetAllNotionParentNotesResponse>(
                            logger,
                            cancellationToken
                        );

            EnsureApiSuccessStatusCode(result);
            EnsureHttpSuccessStatusCode(response);

            return result;
        }
    }
}
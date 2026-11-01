using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestConnectors
    {
        Task<FetchConnectorsResponse> GetMyConnectorsAsync(
            FetchConnectorsRequest request,
            CancellationToken baseCancellationToken
        );

        Task<CreateConnectorResponse<T>> CreateConnectorAsync<T>(
            CreateConnectorRequest<T> request,
            CancellationToken cancellationToken
        ) where T : ConnectorSummary;

        Task<ApiResponse> DeleteConnectorByIdAsync(
            string id,
            CancellationToken cancellationToken
        );
    }
}
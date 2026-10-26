using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Web.Client
{
    public interface IApiRequestExecutions
    {
        Task<FetchExecutionsResponse> GetExecutionsAsync(
            FetchExecutionsRequest request,
            CancellationToken cancellationToken
        );

        Task<ExecutionSummary> GetExecutionByIdAsync(
            string id,
            CancellationToken cancellationToken
        );

        Task<CreateExecutionResponse> CreateExecutionAsync(
            CreateExecutionRequest request,
            CancellationToken cancellationToken
        );

        Task<StartExecutionResponse> StartExecutionAsync(
            string executionId,
            CancellationToken cancellationToken
        );

        Task<CancelExecutionResponse> CancelExecutionAsync(
            string executionId,
            CancellationToken cancellationToken
        );

        Task<DeleteExecutionResponse> DeleteExecutionByIdAsync(
            string executionId,
            CancellationToken cancellationToken
        );
    }
}
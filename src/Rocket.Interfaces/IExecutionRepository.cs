using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Executions;

namespace Rocket.Interfaces
{
    public interface IExecutionRepository
    {
        Task<(IEnumerable<Execution> records, long totalRecordCount)> FetchExecutionsAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        );

        Task<bool> DeleteExecutionAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<Execution> GetExecutionByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<Execution> InsertExecutionAsync(
            Execution execution,
            CancellationToken cancellationToken
        );

        Task<Execution> UpdateExecutionStepAsync<TExecutionStep>(
            string executionStepId,
            string executionId,
            string userId,
            TExecutionStep updatedExecutionStep,
            CancellationToken cancellationToken
        ) where TExecutionStep : BaseExecutionStep;
    }
}
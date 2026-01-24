using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;
using Rocket.Domain.Executions;

namespace Rocket.Interfaces
{
    public interface IExecutionRepository
    {
        Task<(IEnumerable<Execution> records, long totalRecordCount)> FetchExecutionsAsync(
            string userId,
            int? startIndex,
            int? recordCount,
            string scanId,
            string workflowId,
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

        Task UpdateExecutionFieldAsync<TField>(
            string executionId,
            string userId,
            Expression<Func<Execution, TField>> setter,
            TField value,
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
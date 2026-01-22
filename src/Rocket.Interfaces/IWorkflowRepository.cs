using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Workflows;

namespace Rocket.Interfaces
{
    public interface IWorkflowRepository
    {
        Task<bool> WorkflowExistsForNameAsync(
            string userId,
            string workflowName,
            CancellationToken cancellationToken
        );

        Task<bool> WorkflowExistsForMatchingSymbolAsync(
            string userId,
            int matchingPageSymbol,
            CancellationToken cancellationToken
        );

        Task<Workflow> GetWorkflowByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<(IEnumerable<Workflow> records, long totalRecordCount)> FetchWorkflowsAsync(
            string userId,
            int? startIndex,
            int? recordCount,
            CancellationToken cancellationToken
        );

        Task<Workflow> InsertWorkflowAsync(
            Workflow workflow,
            CancellationToken cancellationToken
        );

        Task UpdateWorkflowFieldAsync<TField>(
            string workflowId,
            string userId,
            Expression<Func<Workflow, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        );

        Task<bool> DeleteWorkflowAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );
    }
}
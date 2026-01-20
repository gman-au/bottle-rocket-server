using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public static class WorkflowExecutionTask
    {
        public static async Task<ExecutionStepArtifact> AsTask(
            this BaseExecutionStep step,
            string userId,
            IWorkflowExecutionContext context,
            CancellationToken cancellationToken
        )
        {
            // base execution step should have the connection ID
            // we only need the applicable hook
            var hook =
                context
                    .GetApplicableHook(step);

            return
                await
                    hook
                        .ProcessAsync(
                            context,
                            step,
                            userId,
                            cancellationToken
                        );
        }
    }
}
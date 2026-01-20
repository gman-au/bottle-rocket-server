using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
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

            var connector =
                await
                    context
                        .GetConnectorAsync(userId, step, cancellationToken);

            if (connector == null)
                throw new RocketException(
                    $"Connector has not been defined for step {step.StepName}",
                    ApiStatusCodeEnum.WorkflowMissingConnection
                );

            var artifact =
                context
                    .GetInputArtifact();

            return
                await
                    hook
                        .ProcessAsync(artifact, cancellationToken);
        }
    }
}
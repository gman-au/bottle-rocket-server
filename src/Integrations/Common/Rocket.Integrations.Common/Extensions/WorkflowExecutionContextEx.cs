using System.Threading;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Interfaces;

namespace Rocket.Integrations.Common.Extensions
{
    public static class WorkflowExecutionContextEx
    {
        public static IWorkflowExecutionContext InitializeStep<TExecutionStep>(
            this IWorkflowExecutionContext context,
            HookBase<TExecutionStep> hook,
            BaseExecutionStep step
        )
            where TExecutionStep : BaseExecutionStep
        {
            hook
                .SetExecutionStep(step);

            return context;
        }

        public static IWorkflowExecutionContext InitializeArtifact<TExecutionStep>(
            this IWorkflowExecutionContext context,
            HookBase<TExecutionStep> hook
        )
            where TExecutionStep : BaseExecutionStep
        {
            var artifact =
                context
                    .GetInputArtifact();

            hook
                .SetArtifact(artifact);

            return context;
        }

        public static IWorkflowExecutionContext InitializeConnector<TExecutionStep, TConnector>(
            this IWorkflowExecutionContext context,
            HookWithConnectorBase<TExecutionStep, TConnector> hook,
            string userId,
            BaseExecutionStep step,
            CancellationToken cancellationToken
        )
            where TExecutionStep : BaseExecutionStep
            where TConnector : BaseConnector
        {
            var connector =
                context
                    .GetConnectorAsync<TConnector>(
                        userId,
                        step,
                        cancellationToken
                    )
                    .Result;

            hook
                .SetConnector(connector);

            return context;
        }
    }
}
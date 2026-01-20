using System;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public static class WorkflowExecutionTask
    {
        public static async Task AsTask(
            this BaseExecutionStep step,
            string userId,
            string executionId,
            IWorkflowExecutionContext context,
            Func<string, string, int, BaseExecutionStep, Task> updateExecutionStepCallbackFunc,
            CancellationToken cancellationToken
        )
        {
            try
            {
                // base execution step should have the connection ID
                // we only need the applicable hook
                var hook =
                    context
                        .GetApplicableHook(step);

                var artifactResult =
                    await
                        hook
                            .ProcessAsync(
                                context,
                                step,
                                userId,
                                cancellationToken
                            );

                context
                    .SetCurrentArtifact(artifactResult);
                    
                await
                    updateExecutionStepCallbackFunc(
                        userId,
                        executionId,
                        (int)ExecutionStatusEnum.Completed,
                        step
                    );

                foreach (var childStep in step?.ChildSteps ?? [])
                {
                    await
                        childStep
                            .AsTask(
                                userId,
                                executionId,
                                context,
                                updateExecutionStepCallbackFunc,
                                cancellationToken
                            );
                }
            }
            catch (OperationCanceledException)
            {
                await
                    updateExecutionStepCallbackFunc(
                        userId,
                        executionId,
                        (int)ExecutionStatusEnum.Cancelled,
                        step
                    );
                
                throw;
            }
            catch (Exception)
            {
                await
                    updateExecutionStepCallbackFunc(
                        userId,
                        executionId,
                        (int)ExecutionStatusEnum.Errored,
                        step
                    );

                throw;
            }
        }
    }
}
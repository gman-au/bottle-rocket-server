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
            Func<string, string, int, BaseExecutionStep, Task> callbackFunc,
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
                    callbackFunc(
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
                                callbackFunc,
                                cancellationToken
                            );
                }
            }
            catch (OperationCanceledException)
            {
                await
                    callbackFunc(
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
                    callbackFunc(
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
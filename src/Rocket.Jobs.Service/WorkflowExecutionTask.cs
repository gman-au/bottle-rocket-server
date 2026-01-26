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
            Func<string, string, Task> appendLogMessageCallback,
            Func<string, string, int, BaseExecutionStep, Exception, Task> updateExecutionStepCallbackFunc,
            CancellationToken cancellationToken
        )
        {
            try
            {
                await
                    updateExecutionStepCallbackFunc(
                        userId,
                        executionId,
                        (int)ExecutionStatusEnum.Running,
                        step,
                        null
                    );
                
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
                                appendLogMessageCallback,
                                cancellationToken
                            );

                context
                    .SetCurrentArtifact(artifactResult);

                foreach (var childStep in step?.ChildSteps ?? [])
                {
                    cancellationToken
                        .ThrowIfCancellationRequested();
                    
                    await
                        childStep
                            .AsTask(
                                userId,
                                executionId,
                                context,
                                appendLogMessageCallback,
                                updateExecutionStepCallbackFunc,
                                cancellationToken
                            );
                }
                
                await
                    updateExecutionStepCallbackFunc(
                        userId,
                        executionId,
                        (int)ExecutionStatusEnum.Completed,
                        step,
                        null
                    );
            }
            catch (OperationCanceledException)
            {
                await
                    updateExecutionStepCallbackFunc(
                        userId,
                        executionId,
                        (int)ExecutionStatusEnum.Cancelled,
                        step,
                        null
                    );
                
                throw;
            }
            catch (Exception ex)
            {
                await
                    updateExecutionStepCallbackFunc(
                        userId,
                        executionId,
                        (int)ExecutionStatusEnum.Errored,
                        step,
                        ex
                    );

                throw;
            }
        }
    }
}
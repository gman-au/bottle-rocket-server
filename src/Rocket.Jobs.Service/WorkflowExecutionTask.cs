using System;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Integrations.Domain;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public static class WorkflowExecutionTask
    {
        public static async Task AsTask(
            this BaseExecutionStep step,
            string batchId,
            string userId,
            string executionId,
            IWorkflowExecutionContext context,
            IJobControlBuilder jobControlBuilder,
            CancellationToken cancellationToken
        )
        {
            var jobControl =
                jobControlBuilder
                    .Build(
                        batchId,
                        userId,
                        executionId,
                        step
                    );

            try
            {
                await
                    jobControl
                        .UpdateStepStatusCallback(
                            new JobStatusEntry
                            {
                                ExecutionStatus = (int)ExecutionStatusEnum.Running,
                                ScanId = null
                            }
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
                                jobControl,
                                cancellationToken
                            );

                await
                    jobControl
                        .UpdateStepStatusCallback(
                            new JobStatusEntry
                            {
                                ExecutionStatus = (int)ExecutionStatusEnum.Completed,
                                ScanId = null
                            }
                        );

                foreach (var childStep in step?.ChildSteps ?? [])
                {
                    context
                        .SetCurrentArtifact(artifactResult);

                    cancellationToken
                        .ThrowIfCancellationRequested();

                    await
                        childStep
                            .AsTask(
                                batchId,
                                userId,
                                executionId,
                                context,
                                jobControlBuilder,
                                cancellationToken
                            );
                }
            }
            catch (OperationCanceledException)
            {
                await
                    jobControl
                        .UpdateStepStatusCallback(
                            new JobStatusEntry
                            {
                                ExecutionStatus = (int)ExecutionStatusEnum.Cancelled,
                                ScanId = null
                            }
                        );

                throw;
            }
            catch (ChildStepFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                await
                    jobControl
                        .UpdateStepStatusCallback(
                            new JobStatusEntry
                            {
                                ExecutionStatus = (int)ExecutionStatusEnum.Errored,
                                ScanId = null,
                                Exception = ex
                            }
                        );

                throw new ChildStepFailedException();
            }
        }
    }
}
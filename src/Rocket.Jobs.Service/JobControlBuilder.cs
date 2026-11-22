using System;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Integrations.Common;
using Rocket.Integrations.Domain;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class JobControlBuilder(
        ITranscriber transcriber,
        ICaptureNotifier captureNotifier,
        IExecutionRepository executionRepository,
        IExecutionElementRepository executionElementRepository
    ) : IJobControlBuilder
    {
        public IJobControl Build(
            string batchId,
            string userId,
            string executionId,
            BaseExecutionStep executionStep
        )
        {
            var appendLogMessageTask =
                new Func<JobLogEntry, Task>(async jobLogEntry =>
                    {
                        await
                            executionRepository
                                .AppendLogMessageToStepAsync(
                                    executionStep.Id,
                                    executionId,
                                    userId,
                                    jobLogEntry.LogMessage,
                                    jobLogEntry.ScanId,
                                    CancellationToken.None
                                );
                    }
                );

            var updateStepStatusTask =
                new Func<JobStatusEntry, Task>(async jobStatusEntry =>
                    {
                        await
                            UpdateExecutionStepStatusCallback(
                                userId,
                                executionId,
                                jobStatusEntry.ExecutionStatus,
                                executionStep,
                                jobStatusEntry.ScanId,
                                jobStatusEntry.Exception
                            );
                    }
                );
            
            var addTextTranscriptionTask =
                new Func<string, string, Task>(async (scanId, text) =>
                {
                    await
                        transcriber
                            .ApplyTranscriptionAsync(
                                userId,
                                batchId,
                                scanId,
                                executionId,
                                executionStep.StepName,
                                executionStep.RunDate ?? DateTime.UtcNow,
                                text,
                                CancellationToken.None
                            );
                });

            var jobControl =
                new JobControl
                {
                    AppendLogMessageCallback = appendLogMessageTask,
                    UpdateStepStatusCallback = updateStepStatusTask,
                    AddTextTranscriptionCallback = addTextTranscriptionTask
                };

            return jobControl;
        }

        private async Task UpdateExecutionStepStatusCallback(
            string userId,
            string executionId,
            int executionStatus,
            BaseExecutionStep executionStep,
            string scanId,
            Exception exception = null
        )
        {
            executionStep.ExecutionStatus = executionStatus;
            executionStep.RunDate = DateTime.UtcNow;

            var isElementUpdate = !string.IsNullOrEmpty(scanId);

            if (isElementUpdate)
            {
                var element =
                    await
                        executionElementRepository
                            .GetExecutionElementByScanIdAsync(
                                userId,
                                executionId,
                                scanId,
                                CancellationToken.None
                            );

                element.ExecutionStatus = executionStatus;

                await
                    executionElementRepository
                        .UpdateExecutionElementAsync(
                            element.Id,
                            executionId,
                            userId,
                            element,
                            CancellationToken.None
                        );
            }
            
            if (executionStatus == (int)ExecutionStatusEnum.Running || !isElementUpdate)
            {
                // Don't update the step status for a single element, *unless* it has started running
                await
                    executionRepository
                        .UpdateExecutionStepAsync(
                            executionStep.Id,
                            executionId,
                            userId,
                            executionStep,
                            CancellationToken.None
                        );
            }

            if (exception != null)
            {
                var logMessage = exception.Message;

                await
                    executionRepository
                        .AppendLogMessageToStepAsync(
                            executionStep.Id,
                            executionId,
                            userId,
                            logMessage,
                            scanId,
                            CancellationToken.None
                        );
            }

            if (!isElementUpdate)
            {
                await
                    captureNotifier
                        .NotifyNewExecutionUpdateAsync(
                            userId,
                            CancellationToken.None
                        );
            }
            else
            {
                await
                    captureNotifier
                        .NotifyNewExecutionElementUpdateAsync(
                            userId,
                            scanId,
                            executionStatus,
                            CancellationToken.None
                        );
            }
        }
    }
}
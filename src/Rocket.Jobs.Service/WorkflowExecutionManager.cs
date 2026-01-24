using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Core;
using Rocket.Domain.Core.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class WorkflowExecutionManager(
        IBackgroundTaskQueue queue,
        IExecutionRepository executionRepository,
        ILogger<WorkflowExecutionManager> logger,
        IServiceProvider serviceProvider,
        ICaptureNotifier captureNotifier
    ) : IWorkflowExecutionManager
    {
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _cancellationTokenSources = new();

        public async Task<bool> StartExecutionAsync(
            string executionId,
            string userId,
            CancellationToken cancellationToken
        )
        {
            var cts = new CancellationTokenSource();

            if (_cancellationTokenSources.ContainsKey(executionId))
                throw new RocketException(
                    "This task is already running.",
                    ApiStatusCodeEnum.WorkflowExecutionAlreadyRunning
                );

            _cancellationTokenSources
                .TryAdd(
                    executionId,
                    cts
                );

            var execution =
                await
                    executionRepository
                        .GetExecutionByIdAsync(
                            userId,
                            executionId,
                            cts.Token
                        );

            if (execution == null)
                throw new RocketException(
                    $"Execution not found with ID {executionId}",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            await
                executionRepository
                    .UpdateExecutionFieldAsync(
                        executionId,
                        userId,
                        o => o.RunDate,
                        DateTime.UtcNow,
                        cts.Token
                    );

            // update execution status
            await
                UpdateExecutionStatusCallback(
                    userId,
                    executionId,
                    (int)ExecutionStatusEnum.Running
                );

            queue
                .Enqueue(
                    async token =>
                    {
                        try
                        {
                            var context =
                                serviceProvider
                                    .GetRequiredService<IWorkflowExecutionContext>();

                            await
                                context
                                    .SetRootArtifactAsync(
                                        userId,
                                        execution.ScanId,
                                        cts.Token
                                    );

                            using var linkedCts =
                                CancellationTokenSource
                                    .CreateLinkedTokenSource(
                                        token,
                                        cts.Token
                                    );

                            // the task
                            foreach (var childStep in execution.Steps ?? [])
                            {
                                await
                                    childStep
                                        .AsTask(
                                            userId,
                                            executionId,
                                            context,
                                            UpdateExecutionStepStatusCallback,
                                            linkedCts.Token
                                        );
                            }

                            logger
                                .LogInformation(
                                    "Job has completed: {id}",
                                    executionId
                                );

                            // update execution status
                            await
                                UpdateExecutionStatusCallback(
                                    userId,
                                    executionId,
                                    (int)ExecutionStatusEnum.Completed
                                );
                        }
                        catch (OperationCanceledException)
                        {
                            logger
                                .LogInformation(
                                    "Job was cancelled: {id}",
                                    executionId
                                );

                            // update execution status
                            await
                                UpdateExecutionStatusCallback(
                                    userId,
                                    executionId,
                                    (int)ExecutionStatusEnum.Cancelled
                                );
                        }
                        catch (Exception ex)
                        {
                            logger
                                .LogError(
                                    ex,
                                    "Job has failed: {id}",
                                    executionId
                                );

                            // update execution status
                            await
                                UpdateExecutionStatusCallback(
                                    userId,
                                    executionId,
                                    (int)ExecutionStatusEnum.Errored
                                );
                        }
                        finally
                        {
                            _cancellationTokenSources
                                .TryRemove(
                                    executionId,
                                    out _
                                );

                            cts
                                .Dispose();
                        }
                    }
                );

            return true;
        }

        public async Task<bool> CancelExecutionAsync(string executionId)
        {
            if (!_cancellationTokenSources.TryGetValue(
                    executionId,
                    out var cts
                )) return false;

            await
                cts
                    .CancelAsync();

            return true;
        }

        private async Task UpdateExecutionStepStatusCallback(
            string userId,
            string executionId,
            int executionStatus,
            CoreExecutionStep executionStep
        )
        {
            executionStep.ExecutionStatus = executionStatus;
            executionStep.RunDate = DateTime.UtcNow;

            await
                executionRepository
                    .UpdateExecutionStepAsync(
                        executionStep.Id,
                        executionId,
                        userId,
                        executionStep,
                        CancellationToken.None
                    );

            await
                captureNotifier
                    .NotifyNewExecutionUpdateAsync(
                        userId,
                        CancellationToken.None
                    );
        }

        private async Task UpdateExecutionStatusCallback(
            string userId,
            string executionId,
            int executionStatus
        )
        {
            await
                executionRepository
                    .UpdateExecutionFieldAsync(
                        executionId,
                        userId,
                        o => o.ExecutionStatus,
                        executionStatus,
                        CancellationToken.None
                    );

            await
                captureNotifier
                    .NotifyNewExecutionUpdateAsync(
                        userId,
                        CancellationToken.None
                    );
        }
    }
}
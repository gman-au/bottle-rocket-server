using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class WorkflowExecutionManager(
        IBackgroundTaskQueue queue,
        ILogger<WorkflowExecutionManager> logger
    ) : IWorkflowExecutionManager
    {
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _cancellationTokenSources = new();

        public async Task<bool> StartExecutionAsync(string executionId)
        {
            var cts = new CancellationTokenSource();

            if (_cancellationTokenSources.ContainsKey(executionId))
                throw new RocketException(
                    "This task is already running.",
                    ApiStatusCodeEnum.WorkflowExecutionAlreadyRunning
                );

            _cancellationTokenSources
                .TryAdd(executionId, cts);

            queue
                .Enqueue(async token =>
                {
                    try
                    {
                        using var linkedCts =
                            CancellationTokenSource
                                .CreateLinkedTokenSource(token, cts.Token);

                        await
                            Task
                                .Delay(10000, linkedCts.Token);

                        logger
                            .LogInformation("Job has completed: {id}", executionId);

                        // update execution status
                    }
                    catch (OperationCanceledException)
                    {
                        logger
                            .LogInformation("Job was cancelled: {id}", executionId);

                        // update execution status
                    }
                    finally
                    {
                        _cancellationTokenSources
                            .TryRemove(executionId, out _);

                        cts
                            .Dispose();
                    }
                });

            return true;
        }

        public async Task<bool> CancelExecutionAsync(string executionId)
        {
            if (!_cancellationTokenSources.TryGetValue(executionId, out var cts)) return false;

            await
                cts
                    .CancelAsync();

            return true;
        }
    }
}
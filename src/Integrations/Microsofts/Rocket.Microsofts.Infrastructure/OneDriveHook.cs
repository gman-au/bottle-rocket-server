using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneDriveHook(
        IOneDriveUploader oneDriveUploader,
        ILogger<OneDriveHook> logger
    )
        : HookWithConnectorBase<OneDriveUploadExecutionStep, MicrosoftConnector>(logger), IIntegrationHook
    {
        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            Func<string, string, Task> appendLogMessageCallback,
            CancellationToken cancellationToken
        )
        {
            context
                .InitializeStep(
                    this,
                    step
                )
                .InitializeArtifact(this)
                .InitializeConnector(
                    this,
                    userId,
                    step,
                    cancellationToken
                );

            var noteTitle = $"BR_Note_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";
            var fileName = noteTitle + Artifact.FileExtension;

            await
                oneDriveUploader
                    .UploadFileAsync(
                        Connector,
                        fileName,
                        ExecutionStep.Subfolder ?? "/",
                        Artifact.Artifact,
                        cancellationToken
                    );

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
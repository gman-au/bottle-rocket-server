using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Google.Domain;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;

namespace Rocket.Google.Infrastructure
{
    public class GoogleDriveUploadHook(
        IDriveUploadService driveUploadService,
        ILogger<GoogleDriveUploadHook> logger
    )
        : HookWithConnectorBase<GoogleDriveUploadExecutionStep, GoogleConnector>(logger), IIntegrationHook
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

            var fileBytes = 
                Artifact
                    .Artifact;

            await
                driveUploadService
                    .UploadFileAsync(
                        fileBytes,
                        Artifact.FileExtension,
                        ExecutionStep.ParentFolderId,
                        Connector,
                        cancellationToken
                    );

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
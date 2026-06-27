using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Dropbox.Domain;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxHook(
        IDropboxClientManager dropboxClientManager,
        ILogger<DropboxHook> logger
    )
        : HookWithConnectorBase<DropboxUploadExecutionStep, DropboxConnector>(logger), IIntegrationHook
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

            var fileData =
            await
                dropboxClientManager
                    .UploadFileAsync(
                        Connector.AppKey,
                        Connector.AppSecret,
                        Connector.RefreshToken,
                        ExecutionStep.Subfolder,
                        Artifact.FileName,
                        Artifact.FileExtension,
                        Artifact.Artifact,
                        cancellationToken
                    );
            
            await
                appendLogMessageCallback(
                    step.Id,
                    $"File uploaded: {fileData}"
                );

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
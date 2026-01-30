using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneDriveHook(
        IOneDriveUploader oneDriveUploader,
        ILogger<OneDriveHook> logger
    ) : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is OneDriveUploadExecutionStep;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            Func<string, string, Task> appendLogMessageCallback,
            CancellationToken cancellationToken
        )
        {
            var artifact =
                context
                    .GetInputArtifact();

            var connector =
                await
                    context
                        .GetConnectorAsync<MicrosoftConnector>(
                            userId,
                            step,
                            cancellationToken
                        );

            if (step is not OneDriveUploadExecutionStep oneDriveStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            var noteTitle = $"BR_Note_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";
            var fileName = noteTitle + artifact.FileExtension;

            await
                oneDriveUploader
                    .UploadFileAsync(
                        connector,
                        fileName,
                        oneDriveStep.Subfolder ?? "/",
                        artifact.Artifact,
                        cancellationToken
                    );

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
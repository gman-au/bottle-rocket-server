using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Google.Domain;
using Rocket.Interfaces;

namespace Rocket.Google.Infrastructure
{
    public class GoogleDriveUploadHook(
        ILogger<GoogleDriveUploadHook> logger,
        IDriveUploadService driveUploadService
    ) : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is GoogleDriveUploadExecutionStep;

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
                        .GetConnectorAsync<GoogleConnector>(
                            userId,
                            step,
                            cancellationToken
                        );

            var fileBytes = artifact.Artifact;

            if (step is not GoogleDriveUploadExecutionStep gcpUploadStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            await
                driveUploadService
                    .UploadFileAsync(
                        fileBytes,
                        artifact.FileExtension,
                        connector,
                        cancellationToken
                    );

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
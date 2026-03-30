using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Local.Domain;

namespace Rocket.Local.Infrastructure
{
    public class LocalUploadHook(
        IFileWriter fileWriter,
        ILogger<LocalUploadHook> logger
    )
        : HookBase<LocalUploadExecutionStep>(logger), IIntegrationHook
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
                .InitializeArtifact(this);

            var fileBytes =
                Artifact
                    .Artifact;

            var filePath =
                ExecutionStep
                    .UploadPath;

            var fileName = $"{Guid.NewGuid()}{Artifact.FileExtension}";

            var fullFilePath =
                Path
                    .Combine(
                        filePath,
                        fileName
                    );

            await
                fileWriter
                    .WriteAsync(
                        fullFilePath,
                        fileBytes,
                        cancellationToken
                    );

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
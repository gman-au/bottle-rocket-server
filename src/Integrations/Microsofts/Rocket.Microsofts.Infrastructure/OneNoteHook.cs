using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneNoteHook(
        IOneNoteUploader oneNoteUploader,
        ILogger<OneDriveHook> logger
    )
        : HookWithConnectorBase<OneNoteUploadExecutionStep, MicrosoftConnector>(logger), IIntegrationHook
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

            if (string.IsNullOrEmpty(ExecutionStep.SectionId))
                throw new RocketException(
                    "Section ID not found for OneNote workflow step",
                    ApiStatusCodeEnum.ValidationError
                );

            var noteTitle = $"BR_Note_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";

            if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.RawTextData)
            {
                var textBytes =
                    Artifact
                        .Artifact;

                var textData =
                    Encoding
                        .Default
                        .GetString(textBytes);

                await
                    oneNoteUploader
                        .UploadTextNoteAsync(
                            Connector,
                            ExecutionStep.SectionId,
                            noteTitle,
                            textData,
                            cancellationToken
                        );
            }
            else if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.ImageData)
            {
                await
                    oneNoteUploader
                        .UploadImageNoteAsync(
                            Connector,
                            ExecutionStep.SectionId,
                            Artifact.FileExtension,
                            noteTitle,
                            Artifact.Artifact,
                            cancellationToken
                        );
            }

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
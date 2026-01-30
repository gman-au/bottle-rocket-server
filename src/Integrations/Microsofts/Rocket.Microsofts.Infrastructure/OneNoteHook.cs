using System;
using System.Text;
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
    public class OneNoteHook(
        IOneNoteUploader oneNoteUploader,
        ILogger<OneNoteHook> logger
    ) : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is OneNoteUploadExecutionStep;

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

            if (step is not OneNoteUploadExecutionStep oneNoteStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            if (string.IsNullOrEmpty(oneNoteStep.SectionId))
                throw new RocketException(
                    "Section ID not found for OneNote workflow step",
                    ApiStatusCodeEnum.ValidationError
                );

            var noteTitle = $"BR_Note_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";

            if (artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.RawTextData)
            {
                var textBytes =
                    artifact
                        .Artifact;

                var textData =
                    Encoding
                        .Default
                        .GetString(textBytes);

                await
                    oneNoteUploader
                        .UploadTextNoteAsync(
                            connector,
                            oneNoteStep.SectionId,
                            noteTitle,
                            textData,
                            cancellationToken
                        );
            }
            else if (artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.ImageData)
            {
                await
                    oneNoteUploader
                        .UploadImageNoteAsync(
                            connector,
                            oneNoteStep.SectionId,
                            artifact.FileExtension,
                            noteTitle,
                            artifact.Artifact,
                            cancellationToken
                        );
            }

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
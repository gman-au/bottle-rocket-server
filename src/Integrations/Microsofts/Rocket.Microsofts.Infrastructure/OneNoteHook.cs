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
            IJobControl jobControl,
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

            var elements = Artifact.Elements ?? [];

            var allCompleted = true;

            foreach (var element in elements)
            {
                try
                {
                    await
                        jobControl
                            .StartStepAsync(
                                element.ScanId
                            );

                    var noteTitle = Artifact.FileName ?? $"BR_Note_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";

                    if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.RawTextData)
                    {
                        var textBytes =
                            element
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
                                    element.FileExtension,
                                    noteTitle,
                                    element.Artifact,
                                    cancellationToken
                                );
                    }
                    
                    await
                        jobControl
                            .SucceedStepAsync(
                                $"File {element.FileName} uploaded to OneNote",
                                element.ScanId
                            );
                }
                catch (Exception ex)
                {
                    allCompleted = false;

                    await
                        jobControl
                            .FailStepAsync(
                                ex,
                                element.ScanId
                            );
                }
            }

            if (!allCompleted)
            {
                throw new RocketException(
                    "There was an error uploading the data to OneNote",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            }

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
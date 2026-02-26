using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionHook(
        ILogger<NotionHook> logger,
        INotionNoteUploader notionNoteUploader,
        INotionImageUploader notionImageUploader
    ) : HookWithConnectorBase<NotionUploadExecutionStep, NotionConnector>(logger), IIntegrationHook
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

            if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.RawTextData)
            {
                var textData =
                    ArtifactAsText();

                await
                    notionNoteUploader
                        .UploadTextNoteAsync(
                            Connector.IntegrationSecret,
                            ExecutionStep.ParentNoteId,
                            null,
                            textData,
                            cancellationToken
                        );
            }
            else if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.ImageData)
            {
                var imageBytes =
                    ArtifactAsBytes();

                await
                    notionImageUploader
                        .UploadImageNoteAsync(
                            Connector.IntegrationSecret,
                            ExecutionStep.ParentNoteId,
                            null,
                            imageBytes,
                            Artifact.FileExtension,
                            cancellationToken
                        );
            }

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
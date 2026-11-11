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
    public class NotionNoteUploadHook(
        INotionNoteUploader notionNoteUploader,
        INotionImageUploader notionImageUploader,
        ILogger<NotionNoteUploadHook> logger
    ) : HookWithConnectorBase<NotionUploadNoteExecutionStep, NotionConnector>(logger), IIntegrationHook
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

            var elements =
                Artifact
                    .Elements;
            
            if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.RawTextData)
            {
                await
                    notionNoteUploader
                        .UploadTextNoteAsync(
                            Connector.IntegrationSecret,
                            ExecutionStep.ParentNoteId,
                            elements,
                            jobControl,
                            cancellationToken
                        );
            }
            else if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.ImageData)
            {
                await
                    notionImageUploader
                        .UploadImageNoteAsync(
                            Connector.IntegrationSecret,
                            ExecutionStep.ParentNoteId,
                            elements,
                            jobControl,
                            cancellationToken
                        );
            }

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
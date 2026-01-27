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
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionHook(
        ILogger<NotionHook> logger,
        INotionNoteUploader notionNoteUploader
    ) : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is NotionUploadExecutionStep;

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
                        .GetConnectorAsync<NotionConnector>(
                            userId,
                            step,
                            cancellationToken
                        );
            
            if (step is not NotionUploadExecutionStep notionStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

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
                    notionNoteUploader
                        .UploadTextNoteAsync(
                            connector.IntegrationSecret,
                            notionStep.ParentNoteId,
                            null,
                            textData,
                            cancellationToken
                        );
            }

            // Do the stuff here

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
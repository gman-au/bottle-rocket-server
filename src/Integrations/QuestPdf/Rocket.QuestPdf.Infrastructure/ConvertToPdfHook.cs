using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.QuestPdf.Domain;

namespace Rocket.QuestPdf.Infrastructure
{
    public class ConvertToPdfHook(
        IPdfGenerator pdfGenerator,
        ILogger<ConvertToPdfHook> logger
    )
        : HookBase<ConvertToPdfExecutionStep>(logger), IIntegrationHook
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

            byte[] fileData = null;

            var fileBytes =
                Artifact
                    .Artifact;

            if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.RawTextData)
            {
                var textData =
                    Encoding
                        .Default
                        .GetString(fileBytes);

                fileData =
                    await
                        pdfGenerator
                            .GeneratePdfFromTextAsync(
                                textData,
                                cancellationToken
                            );
            }
            else if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.ImageData)
            {
                fileData =
                    await
                        pdfGenerator
                            .GeneratePdfFromImageAsync(
                                fileBytes,
                                cancellationToken
                            );
            }

            return
                fileData
                    .AsCompletedPdfArtifact();
        }
    }
}
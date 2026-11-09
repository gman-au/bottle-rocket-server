using System.Collections.Generic;
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
            IJobControl jobControl,
            CancellationToken cancellationToken
        )
        {
            context
                .InitializeStep(
                    this,
                    step
                )
                .InitializeArtifact(this);

            IEnumerable<(string, byte[])> fileResults = null;

            var elements =
                Artifact
                    .Elements;

            var mergeFiles = ExecutionStep.MergeFiles;
            
            if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.RawTextData)
            {
                fileResults =
                    await
                        pdfGenerator
                            .GeneratePdfFromTextAsync(
                                jobControl,
                                elements,
                                mergeFiles,
                                cancellationToken
                            );
            }
            else if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.ImageData)
            {
                fileResults =
                    await
                        pdfGenerator
                            .GeneratePdfFromImageAsync(
                                jobControl,
                                elements,
                                mergeFiles,
                                cancellationToken
                            );
            }

            return
                fileResults
                    .AsCompletedPdfArtifact(Artifact);
        }
    }
}
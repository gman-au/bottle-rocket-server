using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.QuestPdf.Domain;

namespace Rocket.QuestPdf.Infrastructure
{
    public class ConvertToPdfHook(IPdfGenerator pdfGenerator) : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is ConvertToPdfExecutionStep;

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

            if (step is not ConvertToPdfExecutionStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            byte[] fileData = null;

            var fileBytes =
                artifact
                    .Artifact;

            if (artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.RawTextData)
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
            else if (artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.ImageData)
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
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.File,
                    Artifact = fileData,
                    FileExtension = ".pdf"
                };
        }
    }
}
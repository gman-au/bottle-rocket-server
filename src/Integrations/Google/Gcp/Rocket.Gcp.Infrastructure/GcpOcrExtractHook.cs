using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Gcp.Domain;
using Rocket.Interfaces;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpOcrExtractHook(
        ILogger<GcpOcrExtractHook> logger,
        IVisionOcrService visionOcrService
    ) : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is GcpExtractExecutionStep;

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
                        .GetConnectorAsync<GcpConnector>(
                            userId,
                            step,
                            cancellationToken
                        );

            var imageBytes = artifact.Artifact;

            if (step is not GcpExtractExecutionStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            var credential = 
                connector
                    .Credential;

            var result =
                await
                    visionOcrService
                        .ExtractHandwrittenTextAsync(
                            imageBytes,
                            credential,
                            cancellationToken
                        );

            await
                appendLogMessageCallback(
                    step.Id,
                    result
                );
            
            var resultArtifact =
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    Artifact =
                        Encoding
                            .Default
                            .GetBytes(
                                result
                            ),
                    FileExtension = ".txt"
                };

            return resultArtifact;
        }
    }
}
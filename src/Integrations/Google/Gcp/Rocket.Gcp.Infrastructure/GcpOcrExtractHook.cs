using System;
using System.Net.Http;
using System.Net.Http.Json;
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
        ILogger<GcpOcrExtractHook> logger
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

            if (step is not GcpExtractExecutionStep ollamaStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            // do the stuff            

            return 
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
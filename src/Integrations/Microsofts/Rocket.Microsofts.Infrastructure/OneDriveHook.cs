using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneDriveHook(
        IOneDriveUploader oneDriveUploader,
        ILogger<OneDriveHook> logger
    )
        : HookWithConnectorBase<OneDriveUploadExecutionStep, MicrosoftConnector>(logger), IIntegrationHook
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
            
            await
                oneDriveUploader
                    .UploadFileAsync(
                        Connector,
                        jobControl,
                        elements,
                        ExecutionStep.Subfolder ?? "/",
                        cancellationToken
                    );

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
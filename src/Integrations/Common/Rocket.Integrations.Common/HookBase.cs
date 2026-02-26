using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.Integrations.Common
{
    public abstract class HookBase<TExecutionStep, TConnector>(ILogger logger)
        where TExecutionStep : BaseExecutionStep
        where TConnector : BaseConnector

    {
        protected ExecutionStepArtifact Artifact;
        protected TExecutionStep ExecutionStep;
        protected TConnector Connector;

        public virtual bool IsApplicable(BaseExecutionStep step) => step is TExecutionStep;

        protected virtual async Task InitializeHookElementsAsync(
            string userId,
            BaseExecutionStep step,
            IWorkflowExecutionContext context,
            CancellationToken cancellationToken
        )
        {
            if (step is not TExecutionStep typedStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            ExecutionStep = typedStep;
            
            Artifact =
                context
                    .GetInputArtifact();

            logger
                .LogDebug(
                    "Artifact of type {fileExtension} set for hook type {type}",
                    Artifact.FileExtension,
                    typeof(TExecutionStep).Name
                );

            var connector =
                await
                    context
                        .GetConnectorAsync<TConnector>(
                            userId,
                            step,
                            cancellationToken
                        );

            Connector =
                connector ??
                throw new RocketException(
                    $"Unexpected connector provided to hook [{typeof(TExecutionStep)}: {typeof(TConnector)}], please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            logger
                .LogDebug(
                    "Connector of type {connector} set for hook type {type}",
                    typeof(TConnector).Name,
                    typeof(TExecutionStep).Name
                );
        }

        protected string ArtifactAsText()
        {
            var textBytes = 
                Artifact
                    .Artifact;

            if (textBytes == null)
                throw new RocketException(
                    $"Artifact data for hook [{typeof(TExecutionStep)}: {typeof(TConnector)}] is empty or not initialized; please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );
            
            return                 
                Encoding
                    .Default
                    .GetString(textBytes);
        }

        protected byte[] ArtifactAsBytes()
        {
            var bytes = 
                Artifact
                    .Artifact;

            if (bytes == null)
                throw new RocketException(
                    $"Artifact data for hook [{typeof(TExecutionStep)}: {typeof(TConnector)}] is empty or not initialized; please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            return
                bytes;
        }
    }
}
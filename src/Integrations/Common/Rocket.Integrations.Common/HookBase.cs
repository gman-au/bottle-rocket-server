using System.Text;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;

namespace Rocket.Integrations.Common
{
    public abstract class HookBase<TExecutionStep>
        where TExecutionStep : BaseExecutionStep
    {
        protected readonly ILogger Logger;

        protected ExecutionStepArtifact Artifact;
        protected TExecutionStep ExecutionStep;

        protected HookBase(ILogger logger)
        {
            Logger = logger;
        }

        public virtual bool IsApplicable(BaseExecutionStep step) => step is TExecutionStep;

        public void SetExecutionStep(BaseExecutionStep step)
        {
            if (step is not TExecutionStep typedStep)
                throw new RocketException(
                    "Unexpected step format, please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            ExecutionStep = typedStep;
        }

        public void SetArtifact(ExecutionStepArtifact artifact)
        {
            Artifact =
                artifact;

            Logger
                .LogDebug(
                    "Artifact of type {fileExtension} set for hook type {type}",
                    Artifact?.FileExtension,
                    typeof(TExecutionStep).Name
                );
        }

        protected string GetArtifactAsText()
        {
            var textBytes =
                Artifact
                    .Artifact;

            if (textBytes == null)
                throw new RocketException(
                    $"Artifact data for hook [{typeof(TExecutionStep)}] is empty or not initialized; please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            return
                Encoding
                    .Default
                    .GetString(textBytes);
        }

        protected byte[] GetArtifactAsBytes()
        {
            var bytes =
                Artifact
                    .Artifact;

            if (bytes == null)
                throw new RocketException(
                    $"Artifact data for hook [{typeof(TExecutionStep)}] is empty or not initialized; please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            return
                bytes;
        }
    }
}
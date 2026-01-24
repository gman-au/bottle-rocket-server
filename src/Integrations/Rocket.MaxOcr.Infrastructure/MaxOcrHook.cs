using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Core;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
using Rocket.MaxOcr.Domain;

namespace Rocket.MaxOcr.Infrastructure
{
    public class MaxOcrHook() : IIntegrationHook
    {
        public bool IsApplicable(CoreExecutionStep step) => step is MaxOcrExtractExecutionStep;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            CoreExecutionStep step,
            string userId,
            CancellationToken cancellationToken
        )
        {
            var artifact =
                context
                    .GetInputArtifact();

            var connector =
                await
                    context
                        .GetConnectorAsync<MaxOcrConnector>(
                            userId,
                            step,
                            cancellationToken
                        );

           
            // do the stuff here for maxocr
            

            // TODO: create a static .Empty artifact
            return artifact;
        }
    }
}
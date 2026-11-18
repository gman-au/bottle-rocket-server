using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Markdig.Domain;

namespace Rocket.Markdig.Infrastructure
{
    public class ConvertToEpubHook(
        IEpubGenerator epubGenerator,
        IAuthorRetriever authorRetriever,
        ILogger<ConvertToEpubHook> logger
    )
        : HookBase<ConvertToEpubExecutionStep>(logger), IIntegrationHook
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

            var mergeFiles =
                ExecutionStep
                    .MergeFiles;

            var authorName =
                await
                    authorRetriever
                        .RetrieveUserNameAsync(
                            userId,
                            cancellationToken
                        );

            var fileResults =
                await
                    epubGenerator
                        .GenerateEpubFromTextAsync(
                            jobControl,
                            Artifact,
                            mergeFiles,
                            authorName,
                            cancellationToken
                        );

            return
                fileResults
                    .AsCompletedEpubArtifact(Artifact);
        }
    }
}
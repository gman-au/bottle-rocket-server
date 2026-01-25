using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Dropbox.Domain;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxHook(IDropboxClientManager dropboxClientManager) : IIntegrationHook
    {
        public bool IsApplicable(BaseExecutionStep step) => step is DropboxUploadExecutionStep;

        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
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
                        .GetConnectorAsync<DropboxConnector>(
                            userId,
                            step,
                            cancellationToken
                        );

            await
                dropboxClientManager
                    .UploadFileAsync(
                        connector.AppKey,
                        connector.AppSecret,
                        connector.RefreshToken,
                        artifact.FileExtension,
                        artifact.Artifact,
                        cancellationToken
                    );

            // TODO: create a static .Empty artifact
            return artifact;
        }
    }
}
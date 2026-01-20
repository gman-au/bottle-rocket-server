using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class WorkflowExecutionContext(
        IEnumerable<IIntegrationHook> hooks,
        IScannedImageHandler scannedImageHandler,
        IConnectorRepository connectorRepository
    )
        : IWorkflowExecutionContext
    {
        private ExecutionStepArtifact _currentArtifact;

        public async Task SetRootArtifactAsync(
            string userId,
            string scanId,
            CancellationToken cancellationToken
        )
        {
            var (record, imageData) =
                await
                    scannedImageHandler
                        .ReadAsync(userId, scanId, cancellationToken);

            if (record == null)
                throw new RocketException(
                    $"Could not find scanned image with ID {scanId}",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            SetCurrentArtifact(
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.ImageData,
                    Artifact = imageData
                }
            );
        }

        public async Task<T> GetConnectorAsync<T>(
            string userId,
            BaseExecutionStep step,
            CancellationToken cancellationToken
        ) where T : BaseConnector
        {
            var connector =
                await
                    connectorRepository
                        .GetConnectorByIdAsync<T>(
                            userId,
                            step?.ConnectorId,
                            cancellationToken
                        );

            if (connector == null)
                throw new RocketException(
                    $"Connector has not been defined for step {step?.StepName}",
                    ApiStatusCodeEnum.WorkflowMissingConnection
                );

            return connector;
        }

        public void SetCurrentArtifact(ExecutionStepArtifact artifact) => _currentArtifact = artifact;

        public ExecutionStepArtifact GetInputArtifact() => _currentArtifact;

        public IIntegrationHook GetApplicableHook(BaseExecutionStep step)
        {
            return
                hooks
                    .FirstOrDefault(o => o.IsApplicable(step));
        }

        public async Task<BaseConnector> GetConnectorAsync(
            string userId,
            BaseExecutionStep step,
            CancellationToken cancellationToken
        )
        {
            var connectorId = step?.ConnectorId;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByIdAsync<BaseConnector>(
                            userId,
                            connectorId,
                            cancellationToken
                        );

            return connector;
        }
    }
}
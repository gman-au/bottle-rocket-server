using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class WorkflowExecutionContext(
        IEnumerable<IIntegrationHook> hooks,
        IBatchHandler batchHandler,
        IConnectorRepository connectorRepository
    ) : IWorkflowExecutionContext
    {
        private ExecutionStepArtifact _currentArtifact;

        public async Task SetRootArtifactAsync(
            string userId,
            string batchId,
            CancellationToken cancellationToken
        )
        {
            var scanData =
                await
                    batchHandler
                        .ReadAsync(
                            userId,
                            batchId,
                            cancellationToken
                        );

            if (scanData == null)
                throw new RocketException(
                    $"Could not find scanned image data with batch ID: {batchId}",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var elements =
                scanData
                    .Select(o =>
                        new ArtifactDataElement
                        {
                            Artifact = o.Value,
                            FileExtension = o.Key.FileExtension,
                            ScanId = o.Key.Id,
                            FileName = o.Key.Description
                        }
                    );
            
            elements =
                elements
                    .ToArray()
                    .EnumerateUniquely();

            SetCurrentArtifact(
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.ImageData,
                    Elements = elements,
                    FileName = _currentArtifact?.FileName,
                    BatchId = batchId,
                    UserId = userId
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
                    $"Connector has not been defined for step \"{step?.StepName}\"",
                    ApiStatusCodeEnum.WorkflowMissingConnection
                );

            if (connector.DetermineStatus() != ConnectorStatusEnum.Active)
                throw new RocketException(
                    $"Connector is not active for step \"{step?.StepName}\"",
                    ApiStatusCodeEnum.WorkflowInactiveConnector
                );

            return connector;
        }

        public void SetCurrentArtifact(
            ExecutionStepArtifact artifact
        ) => _currentArtifact = artifact;

        public ExecutionStepArtifact GetInputArtifact() => _currentArtifact;

        public IIntegrationHook GetApplicableHook(
            BaseExecutionStep step
        )
        {
            return
                hooks
                    .FirstOrDefault(o => o.IsApplicable(step));
        }
    }
}
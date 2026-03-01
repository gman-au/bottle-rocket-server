using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionProjectTaskUploadHook(
        INotionDataSourceUploader notionDataSourceUploader,
        ILogger<NotionProjectTaskUploadHook> logger
    ) : HookWithConnectorBase<NotionUploadProjectTaskExecutionStep, NotionConnector>(logger), IIntegrationHook
    {
        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            Func<string, string, Task> appendLogMessageCallback,
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

            if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData)
            {
                var schema =
                    GetArtifactAsProjectTaskTrackerData();

                var itemCount =
                    schema
                        .Items
                        .Count();

                var index = 0;
                
                foreach (var item in schema.Items)
                {
                    var dataSourcePairs = new List<Tuple<string, string>>
                    {
                        new(
                            ExecutionStep.ProjectCodeColumn,
                            item.Project
                        ),
                        new(
                            ExecutionStep.TaskColumn,
                            item.Task
                        ),
                        new(
                            ExecutionStep.DueDateColumn,
                            item.DueDate
                        ),
                        new(
                            ExecutionStep.EstTimeColumn,
                            item.EstimatedTime
                        )
                    };

                    await
                        notionDataSourceUploader
                            .UploadDataSourceAsync(
                                Connector.IntegrationSecret,
                                ExecutionStep.DataSourceId,
                                dataSourcePairs,
                                cancellationToken
                            );

                    logger
                        .LogInformation(
                            "Uploaded {index} of {itemCount}",
                            index++,
                            itemCount
                        );
                }
            }

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
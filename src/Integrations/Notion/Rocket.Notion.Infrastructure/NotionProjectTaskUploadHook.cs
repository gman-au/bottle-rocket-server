using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
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

            if (Artifact.ArtifactDataFormat == (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData)
            {
                var allCompleted = true;

                foreach (var element in Artifact.Elements)
                {
                    try
                    {
                        await
                            jobControl
                                .StartStepAsync(
                                    element.ScanId
                                );

                        var schema =
                            element
                                .ArtifactAsProjectData();

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

                        await
                            jobControl
                                .SucceedStepAsync(
                                    $"File {element.FileName} uploaded to Notion",
                                    element.ScanId
                                );
                    }
                    catch (Exception ex)
                    {
                        allCompleted = false;

                        await
                            jobControl
                                .FailStepAsync(
                                    ex,
                                    element.ScanId
                                );
                    }
                }

                if (!allCompleted)
                {
                    throw new RocketException(
                        "There was an error uploading the project data to Notion",
                        ApiStatusCodeEnum.ThirdPartyServiceError
                    );
                }
            }

            return
                ExecutionStepArtifact
                    .Empty;
        }
    }
}
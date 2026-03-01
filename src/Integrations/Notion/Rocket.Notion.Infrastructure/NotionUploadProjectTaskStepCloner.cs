using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionUploadProjectTaskStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<NotionUploadProjectTaskWorkflowStep, NotionUploadProjectTaskExecutionStep>(serviceProvider)
    {
        public override NotionUploadProjectTaskExecutionStep Clone(NotionUploadProjectTaskWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.DataSourceId = value.DataSourceId;
            result.ProjectCodeColumn = value.ProjectCodeColumn;
            result.TaskColumn = value.TaskColumn;
            result.DueDateColumn = value.DueDateColumn;
            result.EstTimeColumn = value.EstTimeColumn;

            return result;
        }
    }
}
using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Notion.Contracts;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionUploadProjectTaskExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<NotionUploadProjectTaskExecutionStep, NotionUploadProjectTaskExecutionStepSpecifics>(serviceProvider)
    {
        public override NotionUploadProjectTaskExecutionStep For(NotionUploadProjectTaskExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.DataSourceId = value.DataSourceId;
            result.ProjectCodeColumn = value.ProjectCodeColumn;
            result.TaskColumn = value.TaskColumn;
            result.DueDateColumn = value.DueDateColumn;
            result.EstTimeColumn = value.EstTimeColumn;

            return result;
        }

        public override NotionUploadProjectTaskExecutionStepSpecifics From(NotionUploadProjectTaskExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.DataSourceId = value.DataSourceId;
            result.ProjectCodeColumn = value.ProjectCodeColumn;
            result.TaskColumn = value.TaskColumn;
            result.DueDateColumn = value.DueDateColumn;
            result.EstTimeColumn = value.EstTimeColumn;

            return result;
        }
    }
}
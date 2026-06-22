using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Postmark.Contracts;
using Rocket.Postmark.Domain;

namespace Rocket.Postmark.Infrastructure
{
    public class PostmarkSendEmailExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<PostmarkSendEmailExecutionStep, PostmarkSendEmailExecutionStepSpecifics>(serviceProvider)
    {
        public override PostmarkSendEmailExecutionStep For(PostmarkSendEmailExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            /*result.DataSourceId = value.DataSourceId;
            result.ProjectCodeColumn = value.ProjectCodeColumn;
            result.TaskColumn = value.TaskColumn;
            result.DueDateColumn = value.DueDateColumn;
            result.EstTimeColumn = value.EstTimeColumn;*/

            return result;
        }

        public override PostmarkSendEmailExecutionStepSpecifics From(PostmarkSendEmailExecutionStep value)
        {
            var result =
                base
                    .From(value);

            /*result.DataSourceId = value.DataSourceId;
            result.ProjectCodeColumn = value.ProjectCodeColumn;
            result.TaskColumn = value.TaskColumn;
            result.DueDateColumn = value.DueDateColumn;
            result.EstTimeColumn = value.EstTimeColumn;*/

            return result;
        }
    }
}
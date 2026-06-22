using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Postmark.Domain;

namespace Rocket.Postmark.Infrastructure
{
    public class PostmarkSendEmailStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<PostmarkSendEmailWorkflowStep, PostmarkSendEmailExecutionStep>(serviceProvider)
    {
        public override PostmarkSendEmailExecutionStep Clone(PostmarkSendEmailWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

           /* result.DataSourceId = value.DataSourceId;
            result.ProjectCodeColumn = value.ProjectCodeColumn;
            result.TaskColumn = value.TaskColumn;
            result.DueDateColumn = value.DueDateColumn;
            result.EstTimeColumn = value.EstTimeColumn;*/

            return result;
        }
    }
}
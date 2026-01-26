using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionUploadStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<NotionUploadWorkflowStep, NotionUploadExecutionStep>(serviceProvider)
    {
        public override NotionUploadExecutionStep Clone(NotionUploadWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }
    }
}
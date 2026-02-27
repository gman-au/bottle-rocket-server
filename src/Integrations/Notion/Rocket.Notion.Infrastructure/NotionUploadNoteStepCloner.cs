using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionUploadNoteStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<NotionUploadNoteWorkflowStep, NotionUploadNoteExecutionStep>(serviceProvider)
    {
        public override NotionUploadNoteExecutionStep Clone(NotionUploadNoteWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }
    }
}
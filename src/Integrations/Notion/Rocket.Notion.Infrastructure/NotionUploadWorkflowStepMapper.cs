using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Notion.Contracts;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionUploadWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<NotionUploadWorkflowStep, NotionUploadWorkflowStepSpecifics>(serviceProvider)
    {
        public override NotionUploadWorkflowStep For(NotionUploadWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }

        public override NotionUploadWorkflowStepSpecifics From(NotionUploadWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }
    }
}
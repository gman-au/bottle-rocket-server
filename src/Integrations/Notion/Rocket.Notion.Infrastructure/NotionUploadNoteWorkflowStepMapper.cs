using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Notion.Contracts;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionUploadNoteWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<NotionUploadNoteWorkflowStep, NotionUploadNoteWorkflowStepSpecifics>(serviceProvider)
    {
        public override NotionUploadNoteWorkflowStep For(NotionUploadNoteWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }

        public override NotionUploadNoteWorkflowStepSpecifics From(NotionUploadNoteWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }
    }
}
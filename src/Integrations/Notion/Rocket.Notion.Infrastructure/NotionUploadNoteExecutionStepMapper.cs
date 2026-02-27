using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Notion.Contracts;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionUploadNoteExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<NotionUploadNoteExecutionStep, NotionUploadNoteExecutionStepSpecifics>(serviceProvider)
    {
        public override NotionUploadNoteExecutionStep For(NotionUploadNoteExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }

        public override NotionUploadNoteExecutionStepSpecifics From(NotionUploadNoteExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }
    }
}
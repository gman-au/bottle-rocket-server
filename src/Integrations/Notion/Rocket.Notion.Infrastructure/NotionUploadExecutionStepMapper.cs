using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Notion.Contracts;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionUploadExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<NotionUploadExecutionStep, NotionUploadExecutionStepSpecifics>(serviceProvider)
    {
        public override NotionUploadExecutionStep For(NotionUploadExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }

        public override NotionUploadExecutionStepSpecifics From(NotionUploadExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ParentNoteId = value.ParentNoteId;

            return result;
        }
    }
}
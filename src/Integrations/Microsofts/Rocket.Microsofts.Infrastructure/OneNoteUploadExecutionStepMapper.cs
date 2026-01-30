using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneNoteUploadExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OneNoteUploadExecutionStep, OneNoteUploadExecutionStepSpecifics>(serviceProvider)
    {
        public override OneNoteUploadExecutionStep For(OneNoteUploadExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.SectionId = value.ParentNote;

            return result;
        }

        public override OneNoteUploadExecutionStepSpecifics From(OneNoteUploadExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ParentNote = value.SectionId;

            return result;
        }
    }
}
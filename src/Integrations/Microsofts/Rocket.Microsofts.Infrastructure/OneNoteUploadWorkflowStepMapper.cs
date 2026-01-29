using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneNoteUploadWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OneNoteUploadWorkflowStep, OneNoteUploadWorkflowStepSpecifics>(serviceProvider)
    {
        public override OneNoteUploadWorkflowStep For(OneNoteUploadWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ParentNote = value.SectionId;

            return result;
        }

        public override OneNoteUploadWorkflowStepSpecifics From(OneNoteUploadWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.SectionId = value.ParentNote;

            return result;
        }
    }
}
using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneNoteUploadStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OneNoteUploadWorkflowStep, OneNoteUploadExecutionStep>(serviceProvider)
    {
        public override OneNoteUploadExecutionStep Clone(OneNoteUploadWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ParentNote = value.ParentNote;

            return result;
        }
    }
}
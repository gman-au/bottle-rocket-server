using System;
using Rocket.Infrastructure.Mapping;
using Rocket.QuestPdf.Contracts;
using Rocket.QuestPdf.Domain;

namespace Rocket.QuestPdf.Infrastructure
{
    public class ConvertToPdfWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<ConvertToPdfWorkflowStep, ConvertToPdfWorkflowStepSpecifics>(serviceProvider)
    {
        public override ConvertToPdfWorkflowStep For(ConvertToPdfWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override ConvertToPdfWorkflowStepSpecifics From(ConvertToPdfWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}
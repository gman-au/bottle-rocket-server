using System;
using Rocket.Infrastructure.Mapping;
using Rocket.MaxOcr.Contracts;
using Rocket.MaxOcr.Domain;

namespace Rocket.MaxOcr.Infrastructure
{
    public class MaxOcrExtractWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<MaxOcrExtractWorkflowStep, MaxOcrExtractWorkflowStepSpecifics>(serviceProvider)
    {
        public override MaxOcrExtractWorkflowStep For(MaxOcrExtractWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override MaxOcrExtractWorkflowStepSpecifics From(MaxOcrExtractWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}
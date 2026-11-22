using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Contracts.Project;
using Rocket.OpenAi.Domain.Project;

namespace Rocket.OpenAi.Infrastructure.Project
{
    public class OpenAiExtractProjectWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OpenAiExtractProjectWorkflowStep, OpenAiExtractProjectWorkflowStepSpecifics>(serviceProvider)
    {
        public override OpenAiExtractProjectWorkflowStep For(OpenAiExtractProjectWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OpenAiExtractProjectWorkflowStepSpecifics From(OpenAiExtractProjectWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
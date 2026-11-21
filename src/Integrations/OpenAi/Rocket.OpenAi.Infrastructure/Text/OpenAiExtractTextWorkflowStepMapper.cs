using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Contracts.Text;
using Rocket.OpenAi.Domain.Text;

namespace Rocket.OpenAi.Infrastructure.Text
{
    public class OpenAiExtractTextWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OpenAiExtractTextWorkflowStep, OpenAiExtractTextWorkflowStepSpecifics>(serviceProvider)
    {
        public override OpenAiExtractTextWorkflowStep For(OpenAiExtractTextWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OpenAiExtractTextWorkflowStepSpecifics From(OpenAiExtractTextWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
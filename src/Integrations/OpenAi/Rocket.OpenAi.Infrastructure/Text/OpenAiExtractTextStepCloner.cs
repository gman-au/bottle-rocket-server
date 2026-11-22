using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Domain.Text;

namespace Rocket.OpenAi.Infrastructure.Text
{
    public class OpenAiExtractTextStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OpenAiExtractTextWorkflowStep, OpenAiExtractTextExecutionStep>(serviceProvider)
    {
        public override OpenAiExtractTextExecutionStep Clone(OpenAiExtractTextWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
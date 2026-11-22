using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Domain.Retitle;

namespace Rocket.OpenAi.Infrastructure.Retitle
{
    public class OpenAiRetitleFileStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OpenAiRetitleFileWorkflowStep, OpenAiRetitleFileExecutionStep>(serviceProvider)
    {
        public override OpenAiRetitleFileExecutionStep Clone(OpenAiRetitleFileWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ModelName = value.ModelName;
            result.SummarizeAllFiles = value.SummarizeAllFiles;

            return result;
        }
    }
}
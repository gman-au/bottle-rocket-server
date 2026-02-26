using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Domain.Project;

namespace Rocket.Ollama.Infrastructure.Project
{
    public class OllamaExtractProjectStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OllamaExtractProjectWorkflowStep, OllamaExtractProjectExecutionStep>(serviceProvider)
    {
        public override OllamaExtractProjectExecutionStep Clone(OllamaExtractProjectWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.FirstPassModelName = value.FirstPassModelName;
            result.SecondPassModelName = value.SecondPassModelName;

            return result;
        }
    }
}
using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaExtractStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OllamaExtractWorkflowStep, OllamaExtractExecutionStep>(serviceProvider)
    {
        public override OllamaExtractExecutionStep Clone(OllamaExtractWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}
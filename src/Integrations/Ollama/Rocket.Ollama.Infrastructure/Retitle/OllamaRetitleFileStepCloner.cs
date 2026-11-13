using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Domain.Retitle;

namespace Rocket.Ollama.Infrastructure.Retitle
{
    public class OllamaRetitleFileStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OllamaRetitleFileWorkflowStep, OllamaRetitleFileExecutionStep>(serviceProvider)
    {
        public override OllamaRetitleFileExecutionStep Clone(OllamaRetitleFileWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
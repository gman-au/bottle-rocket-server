using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaExtractTextStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OllamaExtractTextWorkflowStep, OllamaExtractTextExecutionStep>(serviceProvider)
    {
        public override OllamaExtractTextExecutionStep Clone(OllamaExtractTextWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
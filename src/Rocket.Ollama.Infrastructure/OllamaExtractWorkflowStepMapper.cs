using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaExtractWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OllamaExtractWorkflowStep, OllamaExtractWorkflowStepSpecifics>(serviceProvider)
    {
        public override OllamaExtractWorkflowStep For(OllamaExtractWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OllamaExtractWorkflowStepSpecifics From(OllamaExtractWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
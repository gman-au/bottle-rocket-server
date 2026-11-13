using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts.Retitle;
using Rocket.Ollama.Domain.Retitle;

namespace Rocket.Ollama.Infrastructure.Retitle
{
    public class OllamaRetitleFileWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OllamaRetitleFileWorkflowStep, OllamaRetitleFileWorkflowStepSpecifics>(serviceProvider)
    {
        public override OllamaRetitleFileWorkflowStep For(OllamaRetitleFileWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OllamaRetitleFileWorkflowStepSpecifics From(OllamaRetitleFileWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
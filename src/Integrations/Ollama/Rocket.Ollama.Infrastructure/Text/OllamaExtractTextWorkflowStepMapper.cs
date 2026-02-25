using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts.Text;
using Rocket.Ollama.Domain.Text;

namespace Rocket.Ollama.Infrastructure.Text
{
    public class OllamaExtractTextWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OllamaExtractTextWorkflowStep, OllamaExtractTextWorkflowStepSpecifics>(serviceProvider)
    {
        public override OllamaExtractTextWorkflowStep For(OllamaExtractTextWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OllamaExtractTextWorkflowStepSpecifics From(OllamaExtractTextWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
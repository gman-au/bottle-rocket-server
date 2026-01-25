using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaExtractExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OllamaExtractExecutionStep, OllamaExtractExecutionStepSpecifics>(serviceProvider)
    {
        public override OllamaExtractExecutionStep For(OllamaExtractExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OllamaExtractExecutionStepSpecifics From(OllamaExtractExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
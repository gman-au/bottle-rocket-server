using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts.Project;
using Rocket.Ollama.Domain.Project;

namespace Rocket.Ollama.Infrastructure.Project
{
    public class OllamaExtractProjectExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OllamaExtractProjectExecutionStep, OllamaExtractProjectExecutionStepSpecifics>(serviceProvider)
    {
        public override OllamaExtractProjectExecutionStep For(OllamaExtractProjectExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OllamaExtractProjectExecutionStepSpecifics From(OllamaExtractProjectExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
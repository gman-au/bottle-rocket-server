using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts.Retitle;
using Rocket.Ollama.Domain.Retitle;

namespace Rocket.Ollama.Infrastructure.Retitle
{
    public class OllamaRetitleFileExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OllamaRetitleFileExecutionStep, OllamaRetitleFileExecutionStepSpecifics>(serviceProvider)
    {
        public override OllamaRetitleFileExecutionStep For(OllamaRetitleFileExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OllamaRetitleFileExecutionStepSpecifics From(OllamaRetitleFileExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
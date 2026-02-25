using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts.Text;
using Rocket.Ollama.Domain.Text;

namespace Rocket.Ollama.Infrastructure.Text
{
    public class OllamaExtractTextExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OllamaExtractTextExecutionStep, OllamaExtractTextExecutionStepSpecifics>(serviceProvider)
    {
        public override OllamaExtractTextExecutionStep For(OllamaExtractTextExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OllamaExtractTextExecutionStepSpecifics From(OllamaExtractTextExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
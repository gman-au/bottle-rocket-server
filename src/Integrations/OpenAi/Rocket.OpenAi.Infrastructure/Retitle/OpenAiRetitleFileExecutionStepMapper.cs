using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Contracts.Retitle;
using Rocket.OpenAi.Domain.Retitle;

namespace Rocket.OpenAi.Infrastructure.Retitle
{
    public class OpenAiRetitleFileExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OpenAiRetitleFileExecutionStep, OpenAiRetitleFileExecutionStepSpecifics>(serviceProvider)
    {
        public override OpenAiRetitleFileExecutionStep For(OpenAiRetitleFileExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OpenAiRetitleFileExecutionStepSpecifics From(OpenAiRetitleFileExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
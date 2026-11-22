using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Contracts.Text;
using Rocket.OpenAi.Domain.Text;

namespace Rocket.OpenAi.Infrastructure.Text
{
    public class OpenAiExtractTextExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OpenAiExtractTextExecutionStep, OpenAiExtractTextExecutionStepSpecifics>(serviceProvider)
    {
        public override OpenAiExtractTextExecutionStep For(OpenAiExtractTextExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;

            return result;
        }

        public override OpenAiExtractTextExecutionStepSpecifics From(OpenAiExtractTextExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;

            return result;
        }
    }
}
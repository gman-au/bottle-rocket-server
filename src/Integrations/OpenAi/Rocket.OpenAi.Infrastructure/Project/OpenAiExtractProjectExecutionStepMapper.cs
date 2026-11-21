using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Contracts.Project;
using Rocket.OpenAi.Domain.Project;

namespace Rocket.OpenAi.Infrastructure.Project
{
    public class OpenAiExtractProjectExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OpenAiExtractProjectExecutionStep, OpenAiExtractProjectExecutionStepSpecifics>(serviceProvider)
    {
        public override OpenAiExtractProjectExecutionStep For(OpenAiExtractProjectExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.FirstPassModelName = value.FirstPassModelName;
            result.SecondPassModelName = value.SecondPassModelName;

            return result;
        }

        public override OpenAiExtractProjectExecutionStepSpecifics From(OpenAiExtractProjectExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.FirstPassModelName = value.FirstPassModelName;
            result.SecondPassModelName = value.SecondPassModelName;

            return result;
        }
    }
}
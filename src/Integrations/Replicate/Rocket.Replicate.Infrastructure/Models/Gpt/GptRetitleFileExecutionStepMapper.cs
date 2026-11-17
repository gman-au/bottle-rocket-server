using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.Gpt;
using Rocket.Replicate.Domain.Models.Gpt;

namespace Rocket.Replicate.Infrastructure.Models.Gpt
{
    public class GptRetitleFileExecutionStepMapper(
        IServiceProvider serviceProvider
    )
        : ExecutionStepModelMapperBase<GptRetitleFileExecutionStep, GptRetitleFileExecutionStepSpecifics>(serviceProvider)
    {
        public override GptRetitleFileExecutionStep For(GptRetitleFileExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.SummarizeAllFiles = value.SummarizeAllFiles;

            return result;
        }

        public override GptRetitleFileExecutionStepSpecifics From(GptRetitleFileExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.SummarizeAllFiles = value.SummarizeAllFiles;

            return result;
        }
    }
}
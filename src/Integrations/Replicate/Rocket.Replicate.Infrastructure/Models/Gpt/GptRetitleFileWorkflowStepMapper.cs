using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.Gpt;
using Rocket.Replicate.Domain.Models.Gpt;

namespace Rocket.Replicate.Infrastructure.Models.Gpt
{
    public class GptRetitleFileWorkflowStepMapper(
        IServiceProvider serviceProvider
    )
        : WorkflowStepModelMapperBase<GptRetitleFileWorkflowStep, GptRetitleFileWorkflowStepSpecifics>(serviceProvider)
    {
        public override GptRetitleFileWorkflowStep For(GptRetitleFileWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.SummarizeAllFiles = value.SummarizeAllFiles;

            return result;
        }

        public override GptRetitleFileWorkflowStepSpecifics From(GptRetitleFileWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.SummarizeAllFiles = value.SummarizeAllFiles;

            return result;
        }
    }
}
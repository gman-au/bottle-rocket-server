using System;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Contracts.Retitle;
using Rocket.OpenAi.Domain.Retitle;

namespace Rocket.OpenAi.Infrastructure.Retitle
{
    public class OpenAiRetitleFileWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OpenAiRetitleFileWorkflowStep, OpenAiRetitleFileWorkflowStepSpecifics>(serviceProvider)
    {
        public override OpenAiRetitleFileWorkflowStep For(OpenAiRetitleFileWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ModelName = value.ModelName;
            result.SummarizeAllFiles = value.SummarizeAllFiles;

            return result;
        }

        public override OpenAiRetitleFileWorkflowStepSpecifics From(OpenAiRetitleFileWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ModelName = value.ModelName;
            result.SummarizeAllFiles = value.SummarizeAllFiles;

            return result;
        }
    }
}
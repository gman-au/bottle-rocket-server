using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Domain.Models.DataLabTo.Markdown;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Markdown
{
    public class DataLabToExtractMarkdownStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<DataLabToExtractMarkdownWorkflowStep, DataLabToExtractMarkdownExecutionStep>(serviceProvider)
    {
        public override DataLabToExtractMarkdownExecutionStep Clone(DataLabToExtractMarkdownWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}
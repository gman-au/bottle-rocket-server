using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Markdig.Domain;

namespace Rocket.Markdig.Infrastructure
{
    public class ConvertToEpubStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<ConvertToEpubWorkflowStep, ConvertToEpubExecutionStep>(serviceProvider)
    {
        public override ConvertToEpubExecutionStep Clone(ConvertToEpubWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.MergeFiles = value.MergeFiles;

            return result;
        }
    }
}
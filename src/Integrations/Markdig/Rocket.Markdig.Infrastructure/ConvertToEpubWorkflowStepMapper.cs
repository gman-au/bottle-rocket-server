using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Markdig.Contracts;
using Rocket.Markdig.Domain;

namespace Rocket.Markdig.Infrastructure
{
    public class ConvertToEpubWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<ConvertToEpubWorkflowStep, ConvertToEpubWorkflowStepSpecifics>(serviceProvider)
    {
        public override ConvertToEpubWorkflowStep For(ConvertToEpubWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.MergeFiles = value.MergeFiles;

            return result;
        }

        public override ConvertToEpubWorkflowStepSpecifics From(ConvertToEpubWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.MergeFiles = value.MergeFiles;

            return result;
        }
    }
}
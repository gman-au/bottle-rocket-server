using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.DataLabTo.Markdown;
using Rocket.Replicate.Domain.Models.DataLabTo.Markdown;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Markdown
{
    public class DataLabToExtractMarkdownWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<DataLabToExtractMarkdownWorkflowStep, DataLabToExtractMarkdownWorkflowStepSpecifics>(serviceProvider);
}
using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.DataLabTo;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Project
{
    public class DataLabToExtractProjectWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<DataLabToExtractProjectWorkflowStep, DataLabToExtractProjectWorkflowStepSpecifics>(serviceProvider);
}
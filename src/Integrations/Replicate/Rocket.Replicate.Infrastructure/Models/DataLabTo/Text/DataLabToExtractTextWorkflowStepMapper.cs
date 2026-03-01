using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.DataLabTo;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Text
{
    public class DataLabToExtractTextWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<DataLabToExtractTextWorkflowStep, DataLabToExtractTextWorkflowStepSpecifics>(serviceProvider);
}
using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.DataLabTo.Project;
using Rocket.Replicate.Domain.Models.DataLabTo.Project;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Project
{
    public class DataLabToExtractProjectExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<DataLabToExtractProjectExecutionStep, DataLabToExtractProjectExecutionStepSpecifics>(serviceProvider);
}
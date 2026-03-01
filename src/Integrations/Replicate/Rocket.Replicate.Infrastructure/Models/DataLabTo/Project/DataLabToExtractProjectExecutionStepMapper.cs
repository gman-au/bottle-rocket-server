using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.DataLabTo;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Project
{
    public class DataLabToExtractProjectExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<DataLabToExtractProjectExecutionStep, DataLabToExtractProjectExecutionStepSpecifics>(serviceProvider);
}
using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.DataLabTo;
using Rocket.Replicate.Domain.Models.DataLabTo;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo
{
    public class DataLabToExtractTextExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<DataLabToExtractTextExecutionStep, DataLabToExtractTextExecutionStepSpecifics>(serviceProvider);
}
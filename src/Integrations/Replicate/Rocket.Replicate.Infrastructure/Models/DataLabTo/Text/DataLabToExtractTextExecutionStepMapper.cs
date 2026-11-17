using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.DataLabTo.Text;
using Rocket.Replicate.Domain.Models.DataLabTo.Text;

namespace Rocket.Replicate.Infrastructure.Models.DataLabTo.Text
{
    public class DataLabToExtractTextExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<DataLabToExtractTextExecutionStep, DataLabToExtractTextExecutionStepSpecifics>(serviceProvider);
}
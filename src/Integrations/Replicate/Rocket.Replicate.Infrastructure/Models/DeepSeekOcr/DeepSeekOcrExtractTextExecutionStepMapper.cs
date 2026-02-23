using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.DeepSeekOcr;
using Rocket.Replicate.Domain.Models.DeepSeekOcr;

namespace Rocket.Replicate.Infrastructure.Models.DeepSeekOcr
{
    public class DeepSeekOcrExtractTextExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<DeepSeekOcrExtractTextExecutionStep, DeepSeekOcrExtractTextExecutionStepSpecifics>(serviceProvider);
}
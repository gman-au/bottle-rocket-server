using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.Gpt;
using Rocket.Replicate.Domain.Models.Gpt;

namespace Rocket.Replicate.Infrastructure.Models.Gpt
{
    public class GptRetitleFileExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<GptRetitleFileExecutionStep, GptRetitleFileExecutionStepSpecifics>(serviceProvider);
}
using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Replicate.Contracts.Models.Gpt;
using Rocket.Replicate.Domain.Models.Gpt;

namespace Rocket.Replicate.Infrastructure.Models.Gpt
{
    public class GptRetitleFileWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<GptRetitleFileWorkflowStep, GptRetitleFileWorkflowStepSpecifics>(serviceProvider);
}
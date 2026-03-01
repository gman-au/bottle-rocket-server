using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts.Project;
using Rocket.Ollama.Domain.Project;

namespace Rocket.Ollama.Infrastructure.Project
{
    public class OllamaExtractProjectWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OllamaExtractProjectWorkflowStep, OllamaExtractProjectWorkflowStepSpecifics>(serviceProvider)
    {
        public override OllamaExtractProjectWorkflowStep For(OllamaExtractProjectWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.FirstPassModelName = value.FirstPassModelName;
            result.SecondPassModelName = value.SecondPassModelName;

            return result;
        }

        public override OllamaExtractProjectWorkflowStepSpecifics From(OllamaExtractProjectWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.FirstPassModelName = value.FirstPassModelName;
            result.SecondPassModelName = value.SecondPassModelName;

            return result;
        }
    }
}
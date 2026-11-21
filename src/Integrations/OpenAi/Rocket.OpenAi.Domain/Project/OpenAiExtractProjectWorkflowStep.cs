using Rocket.Domain.Workflows;

namespace Rocket.OpenAi.Domain.Project
{
    public record OpenAiExtractProjectWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = OpenAiDomainConstants.OpenAiExtractProjectTaskInputTypes;

        public override int OutputType { get; set; } = OpenAiDomainConstants.OpenAiExtractProjectTaskOutputType;

        public override string StepName { get; set; } = "Extract project task data from image using OpenAI model";

        public override string StepCode { get; set; } = OpenAiDomainConstants.OpenAiExtractProjectTasksWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = OpenAiDomainConstants.ConnectorCode;

        public string FirstPassModelName { get; set; }
        
        public string SecondPassModelName { get; set; }
    }
}
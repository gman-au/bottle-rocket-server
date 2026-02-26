using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Ollama.Domain.Project
{
    public record OllamaExtractProjectWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = [(int)WorkflowFormatTypeEnum.ImageData];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData;

        public override string StepName { get; set; } = "Extract project task data from image using Ollama model";

        public override string StepCode { get; set; } = OllamaDomainConstants.OllamaExtractProjectTasksWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = OllamaDomainConstants.ConnectorCode;

        public string FirstPassModelName { get; set; }
        
        public string SecondPassModelName { get; set; }
    }
}
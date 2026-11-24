using Rocket.Domain.Workflows;

namespace Rocket.Ollama.Domain.WeeklyPlan
{
    public record OllamaExtractWeeklyPlanWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = OllamaDomainConstants.OllamaExtractWeeklyPlanTaskInputTypes;

        public override int OutputType { get; set; } = OllamaDomainConstants.OllamaExtractWeeklyPlanTaskOutputType;

        public override string StepName { get; set; } = "Extract weekly plan data from image using Ollama model";

        public override string StepCode { get; set; } = OllamaDomainConstants.OllamaExtractWeeklyPlanWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = OllamaDomainConstants.ConnectorCode;

        public string FirstPassModelName { get; set; }
        
        public string SecondPassModelName { get; set; }
    }
}
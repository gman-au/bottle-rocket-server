using Rocket.Domain.Workflows;

namespace Rocket.OpenAi.Domain.WeeklyPlan
{
    public record OpenAiExtractWeeklyPlanWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = OpenAiDomainConstants.OpenAiExtractWeeklyPlanInputTypes;

        public override int OutputType { get; set; } = OpenAiDomainConstants.OpenAiExtractWeeklyPlanOutputType;

        public override string StepName { get; set; } = "Extract weekly plan from image using OpenAI model";

        public override string StepCode { get; set; } = OpenAiDomainConstants.OpenAiExtractWeeklyPlanWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = OpenAiDomainConstants.ConnectorCode;

        public string ModelName { get; set; }
    }
}
using Rocket.Domain.Workflows;

namespace Rocket.OpenAi.Domain.Retitle
{
    public record OpenAiRetitleFileWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = OpenAiDomainConstants.OpenAiRetitleFileInputTypes;

        public override int OutputType { get; set; } = OpenAiDomainConstants.OpenAiRetitleFileOutputType;

        public override string StepName { get; set; } = "Suggest a title for the file using an OpenAI model";

        public override string StepCode { get; set; } = OpenAiDomainConstants.OpenAiRetitleFileWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = OpenAiDomainConstants.ConnectorCode;

        public string ModelName { get; set; }
    }
}
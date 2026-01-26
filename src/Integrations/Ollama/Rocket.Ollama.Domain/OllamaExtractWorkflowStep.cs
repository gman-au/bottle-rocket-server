using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Ollama.Domain
{
    public record OllamaExtractWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = [(int)WorkflowFormatTypeEnum.ImageData];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.RawTextData;

        public override string StepName { get; set; } = "Extract text from image using Ollama model";

        public override string RequiresConnectorCode { get; set; } = OllamaDomainConstants.ConnectorCode;

        public string ModelName { get; set; }
    }
}
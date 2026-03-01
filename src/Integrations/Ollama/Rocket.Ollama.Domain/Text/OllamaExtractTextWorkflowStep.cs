using Rocket.Domain.Workflows;

namespace Rocket.Ollama.Domain.Text
{
    public record OllamaExtractTextWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = OllamaDomainConstants.OllamaExtractTextInputTypes;

        public override int OutputType { get; set; } = OllamaDomainConstants.OllamaExtractTextOutputType;

        public override string StepName { get; set; } = "Extract raw text from image using Ollama model";

        public override string StepCode { get; set; } = OllamaDomainConstants.OllamaExtractTextWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = OllamaDomainConstants.ConnectorCode;

        public string ModelName { get; set; }
    }
}
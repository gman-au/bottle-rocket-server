using Rocket.Domain.Workflows;

namespace Rocket.Ollama.Domain.Retitle
{
    public record OllamaRetitleFileWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = OllamaDomainConstants.OllamaRetitleFileInputTypes;

        public override int OutputType { get; set; } = OllamaDomainConstants.OllamaRetitleFileOutputType;

        public override string StepName { get; set; } = "Suggest a title for the file using an Ollama model";

        public override string StepCode { get; set; } = OllamaDomainConstants.OllamaRetitleFileWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = OllamaDomainConstants.ConnectorCode;

        public string ModelName { get; set; }
    }
}
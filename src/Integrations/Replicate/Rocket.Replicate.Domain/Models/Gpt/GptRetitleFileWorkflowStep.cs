using Rocket.Domain.Workflows;

namespace Rocket.Replicate.Domain.Models.Gpt
{
    public record GptRetitleFileWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = ReplicateDomainConstants.ReplicateRetitleFileInputTypes;

        public override int OutputType { get; set; } = ReplicateDomainConstants.OllamaRetitleFileOutputType;

        public override string StepName { get; set; } = "Suggest a title for the file using Replicate (model: gpt-5.2)";

        public override string StepCode { get; set; } = ReplicateDomainConstants.GptRetitleFileWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = ReplicateDomainConstants.ConnectorCode;
    }
}
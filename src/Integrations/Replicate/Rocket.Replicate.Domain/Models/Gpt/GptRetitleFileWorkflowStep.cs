using Rocket.Domain.Workflows;

namespace Rocket.Replicate.Domain.Models.Gpt
{
    public record GptRetitleFileWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = ReplicateDomainConstants.ReplicateRetitleFileInputTypes;

        public override int OutputType { get; set; } = ReplicateDomainConstants.ReplicateRetitleFileOutputType;

        public override string StepName
        {
            get => SummarizeAllFiles
                ? "Suggest a summary for all files using Replicate (model: gpt-5.2)"
                : "Suggest a title for the file using Replicate (model: gpt-5.2)";
            set { }
        }

        public override string StepCode { get; set; } = ReplicateDomainConstants.GptRetitleFileWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = ReplicateDomainConstants.ConnectorCode;

        public bool SummarizeAllFiles { get; set; }
    }
}
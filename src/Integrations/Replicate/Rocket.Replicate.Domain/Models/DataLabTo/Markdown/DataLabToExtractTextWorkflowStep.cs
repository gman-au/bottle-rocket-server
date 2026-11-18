using Rocket.Domain.Workflows;

namespace Rocket.Replicate.Domain.Models.DataLabTo.Markdown
{
    public record DataLabToExtractMarkdownWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = ReplicateDomainConstants.ReplicateExtractInputTypes;

        public override int OutputType { get; set; } = ReplicateDomainConstants.ReplicateExtractMarkdownOutputType;

        public override string StepName { get; set; } = "Extract markdown from image using Replicate (model: datalab-to/marker)";

        public override string StepCode { get; set; } = ReplicateDomainConstants.DataLabToExtractMarkdownWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = ReplicateDomainConstants.ConnectorCode;
    }
}
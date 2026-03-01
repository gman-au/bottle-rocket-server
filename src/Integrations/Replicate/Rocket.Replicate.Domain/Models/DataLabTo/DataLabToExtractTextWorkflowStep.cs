using Rocket.Domain.Workflows;

namespace Rocket.Replicate.Domain.Models.DataLabTo
{
    public record DataLabToExtractTextWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = ReplicateDomainConstants.ReplicateExtractInputTypes;

        public override int OutputType { get; set; } = ReplicateDomainConstants.ReplicateExtractTextOutputType;

        public override string StepName { get; set; } = "Extract text from image using Replicate (model: datalab-to/marker)";

        public override string StepCode { get; set; } = ReplicateDomainConstants.DataLabToExtractTextWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = ReplicateDomainConstants.ConnectorCode;
    }
}
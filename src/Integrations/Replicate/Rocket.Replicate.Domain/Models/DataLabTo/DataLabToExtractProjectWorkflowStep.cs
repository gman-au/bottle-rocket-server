using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Replicate.Domain.Models.DataLabTo
{
    public record DataLabToExtractProjectWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = [(int)WorkflowFormatTypeEnum.ImageData];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.RawTextData;

        public override string StepName { get; set; } = "Extract project data from image using Replicate (model: datalab-to/marker)";

        public override string StepCode { get; set; } = ReplicateDomainConstants.DataLabToExtractProjectWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = ReplicateDomainConstants.ConnectorCode;
    }
}
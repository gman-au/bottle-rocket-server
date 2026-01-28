using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Gcp.Domain
{
    public record GcpExtractWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = [(int)WorkflowFormatTypeEnum.ImageData];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.RawTextData;

        public override string StepName { get; set; } = "Extract text from image using Google Cloud Vision API";

        public override string RequiresConnectorCode { get; set; } = GcpDomainConstants.ConnectorCode;
    }
}
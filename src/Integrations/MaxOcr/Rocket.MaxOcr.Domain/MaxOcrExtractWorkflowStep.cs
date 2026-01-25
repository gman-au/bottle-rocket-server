using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.MaxOcr.Domain
{
    public record MaxOcrExtractWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = [(int)WorkflowFormatTypeEnum.ImageData];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.RawTextData;

        public override string StepName { get; set; } = "Extract text from image using Max OCR";

        public override string RequiresConnectorCode { get; set; } = MaxOcrDomainConstants.ConnectorCode;
    }
}
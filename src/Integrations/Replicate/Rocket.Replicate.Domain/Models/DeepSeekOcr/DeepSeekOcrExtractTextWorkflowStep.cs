using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Replicate.Domain.Models.DeepSeekOcr
{
    public record DeepSeekOcrExtractTextWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = [(int)WorkflowFormatTypeEnum.ImageData];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.RawTextData;

        public override string StepName { get; set; } = "Extract text from image using Replicate (model: lucataco/deepseek-ocr)";

        public override string StepCode { get; set; } = ReplicateDomainConstants.DeepSeekOcrExtractTextWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = ReplicateDomainConstants.ConnectorCode;
    }
}
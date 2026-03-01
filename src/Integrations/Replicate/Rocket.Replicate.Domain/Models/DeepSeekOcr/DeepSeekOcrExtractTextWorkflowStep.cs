using Rocket.Domain.Workflows;

namespace Rocket.Replicate.Domain.Models.DeepSeekOcr
{
    public record DeepSeekOcrExtractTextWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = ReplicateDomainConstants.ReplicateExtractInputTypes;

        public override int OutputType { get; set; } = ReplicateDomainConstants.ReplicateExtractTextOutputType;

        public override string StepName { get; set; } = "Extract text from image using Replicate (model: lucataco/deepseek-ocr)";

        public override string StepCode { get; set; } = ReplicateDomainConstants.DeepSeekOcrExtractTextWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = ReplicateDomainConstants.ConnectorCode;
    }
}
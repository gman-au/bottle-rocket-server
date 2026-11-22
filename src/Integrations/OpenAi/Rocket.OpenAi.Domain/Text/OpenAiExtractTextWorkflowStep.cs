using Rocket.Domain.Workflows;

namespace Rocket.OpenAi.Domain.Text
{
    public record OpenAiExtractTextWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = OpenAiDomainConstants.OpenAiExtractTextInputTypes;

        public override int OutputType { get; set; } = OpenAiDomainConstants.OpenAiExtractTextOutputType;

        public override string StepName { get; set; } = "Extract raw text from image using OpenAI model";

        public override string StepCode { get; set; } = OpenAiDomainConstants.OpenAiExtractTextWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = OpenAiDomainConstants.ConnectorCode;

        public string ModelName { get; set; }
    }
}
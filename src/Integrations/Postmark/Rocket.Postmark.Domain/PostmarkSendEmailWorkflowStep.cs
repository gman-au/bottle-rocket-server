using Rocket.Domain.Workflows;

namespace Rocket.Postmark.Domain
{
    public record PostmarkSendEmailWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = PostmarkDomainConstants.PostmarkSendEmailInputTypes;

        public override int OutputType { get; set; } = PostmarkDomainConstants.PostmarkSendEmailOutputType;

        public override string StepName { get; set; } = "Send email using Postmark";

        public override string StepCode { get; set; } = PostmarkDomainConstants.PostmarkSendEmailWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = PostmarkDomainConstants.ConnectorCode;

        public string RecipientAddress { get; set; }
    }
}
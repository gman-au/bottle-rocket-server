using Rocket.Domain.Workflows;

namespace Rocket.Mailgun.Domain
{
    public record MailgunSendEmailWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = MailgunDomainConstants.MailgunSendEmailInputTypes;

        public override int OutputType { get; set; } = MailgunDomainConstants.MailgunSendEmailOutputType;

        public override string StepName { get; set; } = "Send email using Mailgun";

        public override string StepCode { get; set; } = MailgunDomainConstants.MailgunSendEmailWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = MailgunDomainConstants.ConnectorCode;

        public string RecipientAddress { get; set; }
    }
}
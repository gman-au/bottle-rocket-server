using Rocket.Domain.Executions;

namespace Rocket.Mailgun.Domain
{
    public record MailgunSendEmailExecutionStep : BaseExecutionStep
    {
        public string RecipientAddress { get; set; }
    }
}
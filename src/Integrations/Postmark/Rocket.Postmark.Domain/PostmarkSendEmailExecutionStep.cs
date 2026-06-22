using Rocket.Domain.Executions;

namespace Rocket.Postmark.Domain
{
    public record PostmarkSendEmailExecutionStep : BaseExecutionStep
    {
        public string RecipientAddress { get; set; }
    }
}
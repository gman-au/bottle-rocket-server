using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Mailgun.Contracts
{
    public class MailgunSendEmailExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("recipient_address")]
        public string RecipientAddress { get; set; }
    }
}
using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Mailgun.Contracts
{
    public class MailgunSendEmailWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("recipient_address")]
        public string RecipientAddress { get; set; }
    }
}
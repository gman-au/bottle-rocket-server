using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Postmark.Contracts
{
    public class PostmarkSendEmailWorkflowStepSpecifics : WorkflowStepSummary
    {
        [JsonPropertyName("recipient_address")]
        public string RecipientAddress { get; set; }
    }
}
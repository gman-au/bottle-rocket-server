using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Executions;

namespace Rocket.Postmark.Contracts
{
    public class PostmarkSendEmailExecutionStepSpecifics : ExecutionStepSummary
    {
        [JsonPropertyName("recipient_address")]
        public string RecipientAddress { get; set; }
    }
}
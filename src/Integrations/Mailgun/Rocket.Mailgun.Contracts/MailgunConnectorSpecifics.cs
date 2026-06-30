using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Mailgun.Contracts
{
    public class MailgunConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; }
        
        [JsonPropertyName("sender_address")]
        public string SenderAddress { get; set; }
    }
}
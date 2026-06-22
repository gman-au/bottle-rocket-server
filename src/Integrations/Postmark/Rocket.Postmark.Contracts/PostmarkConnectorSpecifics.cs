using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Postmark.Contracts
{
    public class PostmarkConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("server_token")]
        public string ServerToken { get; set; }
        
        [JsonPropertyName("sender_address")]
        public string SenderAddress { get; set; }
    }
}
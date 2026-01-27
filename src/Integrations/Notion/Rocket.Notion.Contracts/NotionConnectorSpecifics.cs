using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Notion.Contracts
{
    public class NotionConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("integration_secret")]
        public string IntegrationSecret { get; set; }
    }
}
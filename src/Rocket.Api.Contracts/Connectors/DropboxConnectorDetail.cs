using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Connectors
{
    public class DropboxConnectorDetail : ConnectorDetail
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
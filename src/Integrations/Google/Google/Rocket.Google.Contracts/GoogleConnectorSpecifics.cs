using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Google.Contracts
{
    public class GoogleConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("credentials_file_base64")]
        public string CredentialsFileBase64 { get; set; }
    }
}
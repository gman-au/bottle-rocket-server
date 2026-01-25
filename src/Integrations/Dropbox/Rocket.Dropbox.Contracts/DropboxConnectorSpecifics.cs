using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Dropbox.Contracts
{
    public class DropboxConnectorSpecifics : ConnectorSummary
    {
        [JsonPropertyName("app_key")]
        public string AppKey { get; set; }

        [JsonPropertyName("app_secret")]
        public string AppSecret { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        
        [JsonPropertyName("authorize_uri")]
        public string AuthorizeUri { get; set; }
    }
}
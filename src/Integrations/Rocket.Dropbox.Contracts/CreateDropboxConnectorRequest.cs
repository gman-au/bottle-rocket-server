using System.Text.Json.Serialization;

namespace Rocket.Dropbox.Contracts
{
    public class CreateDropboxConnectorRequest
    {
        [JsonPropertyName("app_key")]
        public string AppKey { get; set; }
        
        [JsonPropertyName("app_secret")]
        public string AppSecret { get; set; }
    }
}
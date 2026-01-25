using System.Text.Json.Serialization;

namespace Rocket.Dropbox.Contracts
{
    public class FinalizeDropboxConnectorRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("access_code")]
        public string AccessCode { get; set; }
    }
}
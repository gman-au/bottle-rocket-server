using System.Text.Json.Serialization;

namespace Rocket.Google.Contracts
{
    public class GoogleAuthFinalizeRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("access_code")]
        public string AccessCode { get; set; }
    }
}
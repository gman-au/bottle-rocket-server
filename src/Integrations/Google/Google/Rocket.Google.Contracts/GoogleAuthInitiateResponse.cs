using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.Google.Contracts
{
    public class GoogleAuthInitiateResponse : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("authorization_url")]
        public string AuthorizationUrl { get; set; }
    }
}
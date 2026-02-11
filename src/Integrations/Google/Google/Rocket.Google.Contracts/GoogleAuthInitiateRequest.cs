using System.Text.Json.Serialization;

namespace Rocket.Google.Contracts
{
    public class GoogleAuthInitiateRequest
    {
        [JsonPropertyName("credentials_file_base64")]
        public string CredentialsFileBase64 { get; set; }
    }
}
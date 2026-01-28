using System.Text.Json.Serialization;

namespace Rocket.Microsofts.Contracts
{
    public class MicrosoftDeviceCodeResult
    {
        [JsonPropertyName("user_code")]
        public string UserCode { get; set; }

        [JsonPropertyName("device_code")]
        public string DeviceCode { get; set; }

        [JsonPropertyName("verification_url")]
        public string VerificationUrl { get; set; }

        [JsonPropertyName("expires_in")]
        public double ExpiresIn { get; set; }

        [JsonPropertyName("interval")]
        public long Interval { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
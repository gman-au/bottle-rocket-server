using System.Text.Json.Serialization;

namespace Rocket.MaxOcr.Contracts
{
    public class CreateMaxOcrConnectorRequest
    {
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; }
    }
}
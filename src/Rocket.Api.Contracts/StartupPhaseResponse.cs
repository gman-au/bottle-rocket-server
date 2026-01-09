using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts
{
    public class StartupPhaseResponse : ApiResponse
    {
        [JsonPropertyName("phase")]
        public int Phase { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace Rocket.OpenAi.Contracts
{
    public class OpenAiModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
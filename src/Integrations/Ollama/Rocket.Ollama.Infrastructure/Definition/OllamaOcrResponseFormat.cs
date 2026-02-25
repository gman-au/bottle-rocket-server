using System.Text.Json.Serialization;

namespace Rocket.Ollama.Infrastructure.Definition
{
    public class OllamaOcrResponseFormat
    {
        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("schema")] public OllamaJsonSchema JsonSchema { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace Rocket.Ollama.Infrastructure.Definition
{
    public class OllamaJsonSchema
    {
        [JsonPropertyName("schema")] public dynamic Schema { get; set; }
    }
}
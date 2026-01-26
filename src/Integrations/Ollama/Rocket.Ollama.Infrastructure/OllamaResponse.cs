using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaResponse
    {
        [JsonPropertyName("text")] public IEnumerable<string[]> Text { get; set; }
    }
}
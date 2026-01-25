using System.Text.Json.Serialization;

namespace Rocket.Ollama.Infrastructure.Definition
{
    public class OllamaOcrResponse
    {
        [JsonPropertyName("model")] 
        public string Model { get; set; }
        
        [JsonPropertyName("message")] 
        public OllamaOcrResponseMessage Message { get; set; }
    }
}
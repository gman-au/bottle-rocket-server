using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Ollama.Infrastructure.Definition
{
    public class OllamaOcrRequest
    {
        [JsonPropertyName("model")] 
        public string Model { get; set; }
        
        [JsonPropertyName("messages")] 
        public IEnumerable<OllamaOcrRequestMessage> Messages { get; set; }
        
        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
    }
}
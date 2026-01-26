using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Ollama.Infrastructure.Definition
{
    public class OllamaOcrRequestMessage
    {
        [JsonPropertyName("role")] 
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
        
        [JsonPropertyName("images")] 
        public IEnumerable<string> Images { get; set; }
    }
}
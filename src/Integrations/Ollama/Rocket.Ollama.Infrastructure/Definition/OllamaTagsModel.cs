using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Ollama.Infrastructure.Definition
{
    public class OllamaTagsModel
    {
        [JsonPropertyName("name")] 
        public string Name { get; set; }
        
        [JsonPropertyName("model")] 
        public string Model { get; set; }
        
        [JsonPropertyName("capabilities")]
        public IEnumerable<string> Capabilities { get; set; }
    }
}
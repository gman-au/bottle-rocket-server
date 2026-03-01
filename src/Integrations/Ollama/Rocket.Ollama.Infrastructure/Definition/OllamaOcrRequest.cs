using System.Collections.Generic;
using System.Text.Json;
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
        
        [JsonPropertyName("format")]
        public JsonElement? Format { get; set; }

        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; }

        [JsonPropertyName("num_predict")]
        public int? NumPredict { get; set; }
        
        [JsonPropertyName("num_ctx")]
        public int? NumCtx { get; set; }
    }
}
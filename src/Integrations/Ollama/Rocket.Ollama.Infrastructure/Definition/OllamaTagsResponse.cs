using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Ollama.Infrastructure.Definition
{
    public class OllamaTagsResponse
    {
        [JsonPropertyName("models")]
        public IEnumerable<OllamaTagsModel> Models { get; set; }
    }
}
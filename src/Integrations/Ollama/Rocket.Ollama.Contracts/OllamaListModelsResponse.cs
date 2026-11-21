using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.Ollama.Contracts
{
    public class OllamaListModelsResponse : ApiResponse
    {
        [JsonPropertyName("models")]
        public IEnumerable<OllamaModel> Models { get; set; }
    }
}
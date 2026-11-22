using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.OpenAi.Contracts
{
    public class OpenAiListModelsResponse : ApiResponse
    {
        [JsonPropertyName("models")]
        public IEnumerable<OpenAiModel> Models { get; set; }
    }
}
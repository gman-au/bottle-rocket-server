using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.OpenAi.Infrastructure.Definition
{
    public class OpenAiTagsResponse
    {
        [JsonPropertyName("models")]
        public IEnumerable<OpenAiTagsModel> Models { get; set; }
    }
}
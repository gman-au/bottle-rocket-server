using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.MaxOcr.Infrastructure
{
    public class MaxOcrResponse
    {
        [JsonPropertyName("text")] public IEnumerable<string[]> Text { get; set; }
    }
}
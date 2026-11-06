using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Batches
{
    public class CloseBatchRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("workflows")]
        public IEnumerable<string> Workflows { get; set; }
    }
}